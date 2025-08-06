 
using System.Numerics;
using System.Text;
using System.Windows.Forms;
 

namespace EZGLib
{
    public static class VerySimpleGeometry

    {
       public static class Example
        {
            public static void call()
            {
                // create model 
                var model = new VerySimpleGeometry.VSG_Model();
                // load from file
                string path = VerySimpleGeometry.SelectVsgOrObjFile();
                if (path.Length > 0)
                {
                    if (Path.GetExtension(path).ToLower() == ".vsg")
                    {
                        model.Load(path);
                    }
                  else if (Path.GetExtension(path).ToLower() == ".obj")
                    {
                        model.LoadOBJ(path);
                    }

                }
                // save the model 
                string savePath = SaveVsgOrObjFile();
                if (savePath.Length > 0)
                {
                    if (Path.GetExtension(savePath).ToLower() == ".vsg")
                    {
                        model.Save(path);
                    }
                    else if (Path.GetExtension(savePath).ToLower() == ".obj")
                    {
                        model.WriteOBJ(savePath);
                    }

                }
            }
        }
        public static string SaveVsgOrObjFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set up two filters: one for .vsg and one for .obj
                saveFileDialog.Filter = "VSG Files (*.vsg)|*.vsg|OBJ Files (*.obj)|*.obj";
                saveFileDialog.Title = "Save as VSG or OBJ File";
                saveFileDialog.AddExtension = true;
                saveFileDialog.RestoreDirectory = true;

                DialogResult result = saveFileDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    return saveFileDialog.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string SelectVsgOrObjFile()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "VSG and OBJ Files (*.vsg;*.obj)|*.vsg;*.obj";
                openFileDialog.Title = "Select a VSG or OBJ File";

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public class VSG_Model
        {
            public List<VSG_Mesh> meshes;

            public void Save(string path)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var mesh in meshes)
                {
                    sb.AppendLine(mesh.ToString());
                }
                File.WriteAllText(path, sb.ToString());


            }
            public void Load(string path)
            {
                meshes = new List<VSG_Mesh>();
                string content = File.ReadAllText(path);
                int start = 0;

                while ((start = content.IndexOf('<', start)) != -1)
                {
                    int end = content.IndexOf('>', start);
                    if (end == -1) break;

                    string meshData = content.Substring(start + 1, end - start - 1);
                    var parts = meshData.Split('|');

                    // Parse vertex data
                    var vertexTokens = parts[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var vertices = new List<VS_Vertex>();
                    for (int i = 0; i < vertexTokens.Length; i += 8)
                    {
                        float x = float.Parse(vertexTokens[i]);
                        float y = float.Parse(vertexTokens[i + 1]);
                        float z = float.Parse(vertexTokens[i + 2]);
                        float xn = float.Parse(vertexTokens[i + 3]);
                        float yn = float.Parse(vertexTokens[i + 4]);
                        float zn = float.Parse(vertexTokens[i + 5]);
                        float u = float.Parse(vertexTokens[i + 6]);
                        float v = float.Parse(vertexTokens[i + 7]);
                        vertices.Add(new VS_Vertex(x, y, z, xn, yn, zn, u, v));
                    }

                    // Parse triangle indices
                    var triangleTokens = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var triangles = new List<VS_Triangle>();
                    for (int i = 0; i < triangleTokens.Length; i += 3)
                    {
                        int i1 = int.Parse(triangleTokens[i]);
                        int i2 = int.Parse(triangleTokens[i + 1]);
                        int i3 = int.Parse(triangleTokens[i + 2]);
                        triangles.Add(new VS_Triangle(vertices[i1], vertices[i2], vertices[i3]));
                    }

                    // Parse color
                    var colorTokens = parts[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    float r = float.Parse(colorTokens[0]);
                    float g = float.Parse(colorTokens[1]);
                    float b = float.Parse(colorTokens[2]);
                    Vector3 color = new Vector3(r, g, b);

                    // Create mesh
                    var mesh = new VSG_Mesh
                    {
                        vertices = vertices,
                        triangles = triangles,
                        Color = color
                    };
                    meshes.Add(mesh);

                    start = end + 1;
                }
            }
            public void WriteOBJ(string path)
            {
                StringBuilder sb = new StringBuilder();
                int vertexOffset = 1; // OBJ is 1-indexed

                for (int meshIndex = 0; meshIndex < meshes.Count; meshIndex++)
                {
                    VSG_Mesh mesh = meshes[meshIndex];
                    sb.AppendLine($"o Mesh_{meshIndex}");

                    foreach (var v in mesh.vertices)
                    {
                        sb.AppendLine($"v {v.Position.X} {v.Position.Y} {v.Position.Z}");
                    }
                    foreach (var v in mesh.vertices)
                    {
                        sb.AppendLine($"vn {v.Normal.X} {v.Normal.Y} {v.Normal.Z}");
                    }
                    foreach (var v in mesh.vertices)
                    {
                        sb.AppendLine($"vt {v.UV.X} {v.UV.Y}");
                    }

                    foreach (var t in mesh.triangles)
                    {
                        int i1 = mesh.vertices.IndexOf(t.Vertex1) + vertexOffset;
                        int i2 = mesh.vertices.IndexOf(t.Vertex2) + vertexOffset;
                        int i3 = mesh.vertices.IndexOf(t.Vertex3) + vertexOffset;

                        sb.AppendLine($"f {i1}/{i1}/{i1} {i2}/{i2}/{i2} {i3}/{i3}/{i3}");
                    }

                    vertexOffset += mesh.vertices.Count;
                }

                File.WriteAllText(path, sb.ToString());
            }
            public void LoadOBJ(string path)
            {
                meshes = new List<VSG_Mesh>();
                var lines = File.ReadAllLines(path);

                var positions = new List<Vector3>();
                var normals = new List<Vector3>();
                var uvs = new List<Vector2>();

                VSG_Mesh currentMesh = null;
                Dictionary<int, VS_Vertex> vertexCache = new Dictionary<int, VS_Vertex>();

                foreach (string rawLine in lines)
                {
                    string line = rawLine.Trim();

                    if (line.StartsWith("o "))
                    {
                        if (currentMesh != null)
                            meshes.Add(currentMesh);

                        currentMesh = new VSG_Mesh
                        {
                            vertices = new List<VS_Vertex>(),
                            triangles = new List<VS_Triangle>(),
                            Color = new Vector3(255, 255, 255)
                        };
                        vertexCache.Clear();
                    }
                    else if (line.StartsWith("v "))
                    {
                        var parts = line.Split(' ');
                        positions.Add(new Vector3(
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            float.Parse(parts[3])
                        ));
                    }
                    else if (line.StartsWith("vt "))
                    {
                        var parts = line.Split(' ');
                        uvs.Add(new Vector2(
                            float.Parse(parts[1]),
                            float.Parse(parts[2])
                        ));
                    }
                    else if (line.StartsWith("vn "))
                    {
                        var parts = line.Split(' ');
                        normals.Add(new Vector3(
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            float.Parse(parts[3])
                        ));
                    }
                    else if (line.StartsWith("f "))
                    {
                        if (currentMesh == null)
                        {
                            currentMesh = new VSG_Mesh
                            {
                                vertices = new List<VS_Vertex>(),
                                triangles = new List<VS_Triangle>(),
                                Color = new Vector3(255, 255, 255)
                            };
                        }

                        var parts = line.Substring(2).Split(' ');
                        var indices = new List<int>();

                        foreach (var part in parts)
                        {
                            var comps = part.Split('/');
                            int vIdx = int.Parse(comps[0]) - 1;
                            int vtIdx = comps.Length > 1 && comps[1] != "" ? int.Parse(comps[1]) - 1 : vIdx;
                            int vnIdx = comps.Length > 2 && comps[2] != "" ? int.Parse(comps[2]) - 1 : vIdx;

                            // Create a unique key to identify a unique combination
                            int key = (vIdx << 16) ^ (vtIdx << 8) ^ vnIdx;

                            if (!vertexCache.TryGetValue(key, out VS_Vertex vertex))
                            {
                                vertex = new VS_Vertex
                                {
                                    Position = positions[vIdx],
                                    Normal = (vnIdx < normals.Count ? normals[vnIdx] : Vector3.Zero),
                                    UV = (vtIdx < uvs.Count ? uvs[vtIdx] : Vector2.Zero)
                                };
                                currentMesh.vertices.Add(vertex);
                                vertexCache[key] = vertex;
                            }

                            indices.Add(currentMesh.vertices.IndexOf(vertex));
                        }

                        currentMesh.triangles.Add(new VS_Triangle(
                            currentMesh.vertices[indices[0]],
                            currentMesh.vertices[indices[1]],
                            currentMesh.vertices[indices[2]]
                        ));
                    }
                }

                if (currentMesh != null)
                    meshes.Add(currentMesh);
            }


            public class VSG_Mesh
            {
                public Vector3 Color = new Vector3(255, 255, 255);
                public List<VS_Vertex> vertices;
                public List<VS_Triangle> triangles;

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<");
                    foreach (VS_Vertex v in vertices)
                    {
                        sb.Append($"{v.Position.X} {v.Position.Y} {v.Position.Z} {v.Normal.X} {v.Normal.Y} {v.Normal.Z} {v.UV.X} {v.UV.Y}");
                    }
                    sb.Append("|");
                    foreach (var triangle in triangles)
                    {
                        sb.Append($"{vertices.IndexOf(triangle.Vertex1)} {vertices.IndexOf(triangle.Vertex2)} {vertices.IndexOf(triangle.Vertex3)} ");
                    }
                    sb.Append("|");
                    sb.Append($"{Color.X} {Color.Y} {Color.Z}");
                    sb.Append(">");
                    return sb.ToString();
                }
            }
            public class VS_Vertex
            {
                public Vector3 Position = new Vector3();
                public Vector3 Normal = new Vector3();
                public Vector2 UV = new Vector2();
                public VS_Vertex(float x, float y, float z, float xn, float yn, float zn, float u, float v)
                {
                    Position = new Vector3(x, y, z);
                    Normal = new Vector3(xn, yn, zn);
                    UV = new Vector2(u, v);

                }
                public VS_Vertex() { }


            }
            public class VS_Triangle
            {
                public VS_Vertex Vertex1, Vertex2, Vertex3;

                public VS_Triangle(VS_Vertex vertex1, VS_Vertex vertex2, VS_Vertex vertex3)
                {
                    Vertex1 = vertex1;
                    Vertex2 = vertex2;
                    Vertex3 = vertex3;
                }
            }
        }
    }
}