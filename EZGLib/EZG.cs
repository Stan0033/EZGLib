
using System.Numerics;

using System.Text;
using System.Xml.Linq;
 


namespace custom_Format_3d
{
    public static partial class EZGeometry
    {
        public class EZGModel
        {

            public List<EZMesh> Meshes = new List<EZMesh>();
            public List<EZMaterial> Materials = new List<EZMaterial>();
            public List<EZTransformation> Transformations = new List<EZTransformation>();
            public List<EZNode> Nodes = new List<EZNode>();
            public List<EZSequence> Sequences = new List<EZSequence>();
            public string Name = "Unnamed";

          public void Save(string filePath)
            {

            }
            public void Load(string filePath) { }
            public void SaveBinary(string filePath)
            {

            }

                  public void LoadBinary(string filePath) { }
        }
        public class EZMesh
        {
            public List<EZVertex> Vertices = new List<EZVertex>();
            public List<EZTriangle> Triangles = new List<EZTriangle>();
            public EZMaterial? Material;
            public string Name = "Unnamed";
            public EZGModel? Owner;
            public EZMesh(EZGModel owner)
            {
                this.Owner = owner;
            }

            public string _Name => Name.Replace(' ','_').ToString();

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[mesh] {_Name}");
                // get vertices
                foreach (var item in Vertices)
                {
                    sb.Append(item.ToString());
                }
                sb.Append('|');
                // get vertex attachments
                foreach (var item in Vertices)
                {
                    sb.Append($"[{string.Join(" ", item.AttachedTo.Select(x=>Owner.Nodes.IndexOf(x)))}] ");
                }
                sb.Append('|');
                return base.ToString();
                //unfinished
            }
        }
        public class EZNode
        {
            public string Name = string.Empty;
            public Vector3 PivotPoint = new Vector3();
            public EZNode? Parent;
            public EZTransformation? Translation, Rotation, Scaling;
            public EZGModel Owner;
            public bool InheritsTranslation, InheritsScaling,InheritsRotation;
            public EZNode(string name, EZGModel owner)
            {
                Name = name;
                Owner = owner;
            }

            public string _Name { get => Name.Replace(' ', '_').ToString(); }
            public override string ToString()
            {
                return $"[node] {_Name} {Owner.Nodes.IndexOf(Parent)} {PivotPoint.X} {PivotPoint.Y} {PivotPoint.Z} {Owner.Transformations.IndexOf(Translation)} {Owner.Transformations.IndexOf(Rotation)} {Owner.Transformations.IndexOf(Scaling)} {InheritsTranslation} {InheritsRotation} {InheritsScaling}";
            }
        }
        public class EZTransformation
        {
            public Dictionary<int, Vector3> Keyframes = new Dictionary<int, Vector3>();
            public EZGModel? Owner;
            public EZTransformation (EZGModel owner)
            {
                this.Owner = owner;
            }
            public override string ToString()
            {
                // the keyframes are on the same line, in pairs of 4
                if (Keyframes.Count == 0) return "[transformation]";
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("[transformation]");
                    foreach (var kf in Keyframes) 
                    {
                        sb.Append($" {kf.Key} {kf.Value.X} {kf.Value.Y} {kf.Value.Z}");
                    }
                    return sb.ToString();

                }
            }
        }
        public class EZSequence
        {
            public string Name { get; set; } = string.Empty;

            private int from = 0;
            public int From
            {
                get => from;
                set
                {
                    if (value >= To) return;
                    if (value < 0) return;
                    from = value;
                }
            }

            private int to = 0;
            public bool Looping = false;

            public int To
            {
                get => to;
                set
                {
                    if (value <= From) return;
                    if (value < 0) return;
                    to = value;
                }
            }

            public int Interval => To - From;

            public string _Name { get => Name.Replace(' ', '_').ToString(); }

            public EZSequence(string name, int from, int to)
            {
                Name = name;
                this.from = from;  // Use backing fields to avoid validation in constructor
                this.to = to;
            }
            public EZSequence(string name, int from, int to, bool looping)
            {
                Name = name;
                this.from = from;  // Use backing fields to avoid validation in constructor
                this.to = to;
                Looping = looping;
            }
            public bool Contains(int frame)
            {
                return frame >= From && frame <= To;
            }

            public override string ToString()
            {
                return $"[sequence] {_Name} {From} {To} {Looping}";
            }
        }

        public class EZMaterial
        {
            public string Name  = "DefaultMaterial";

            // RGB colors as Vector3 (R, G, B), values from 0 to 1
            public Vector3 DiffuseColor  = new Vector3(1, 1, 1);
            public Vector3 AmbientColor  = new Vector3(0.1f, 0.1f, 0.1f);
            public Vector3 SpecularColor  = new Vector3(1, 1, 1);
            public bool TwoSided = false;
            public bool Unshaded = false;

            // Shininess (specular exponent)
            public float Shininess   = 32f;

            // Transparency (0 = fully transparent, 1 = fully opaque)
            public float Opacity  = 1f;

            // Optional diffuse texture filename (relative or absolute)
            public string DiffuseTexture = string.Empty;
            public EZGModel? Owner;
           
            public EZMaterial(string name, EZGModel owner)
            {
                Name = name;
                Owner = owner;
            }

            public override string ToString()
            {
                return $"[material] {Name} {DiffuseColor.X} {DiffuseColor.Y} {DiffuseColor.Z} " +
                       $"{AmbientColor.X} {AmbientColor.Y} {AmbientColor.Z} " +
                       $"{SpecularColor.X} {SpecularColor.Y} {SpecularColor.Z} " +
                       $"{Shininess} {Opacity} {DiffuseTexture ?? "None"}" +
                       $"{Unshaded} {TwoSided}";
            }

        }

        public class EZTriangle
        {
            public EZVertex? vertex1;
            public EZVertex? vertex2;
            public EZVertex? vertex3;
            private EZMesh Owner;
            public EZTriangle(EZVertex one, EZVertex two, EZVertex trhee, EZMesh owner)
            {
                vertex1 = one;
                vertex2 = two;
                vertex3 = trhee;
                Owner = owner;
            }
            public string GetString( )
            {
                return $"{Owner.Vertices.IndexOf(vertex1)} {Owner.Vertices.IndexOf(vertex2)} {Owner.Vertices.IndexOf(vertex3)}";
            }
        }
        public class EZVertex
        {
            public Vector3 Position = new Vector3();
            public Vector3 Normal = new Vector3();
            public Vector2 UV = new Vector2();
            public List<EZNode> AttachedTo = new List<EZNode>();
            EZMesh? Owner;
            public EZVertex(Vector3 position, Vector3 normal, Vector2 uv, EZMesh? owner)
            {
                this.Position = position;
                this.Normal = normal;
                this.UV = uv;
                Owner = owner;
            }
            public EZVertex(float x, float y, float z, float nx, float ny, float nz, float u, float v, EZMesh owner)
            {
                this.Position = new Vector3(x, y, z);
                this.Normal = new Vector3(nx, ny, nz);
                this.UV = new Vector2(u, v);
                this.Owner = owner;
            }
            
            public override string ToString()
            {
                return $"{Position.X} {Position.Y} {Position.Z} {Normal.X} {Normal.Y} {Normal.Z} {UV.X} {UV.Y}";
            }
        }

       
       
        

        private static bool IsBinary(string fromFile)
        {
            using (var reader = new StreamReader(fromFile))
            {
                int firstChar = reader.Read(); // Reads one character (not the full line)
                return firstChar == 'b';
            }
        }
        public static void ExportAsOBJ(EZGModel model, string savePath)
        {
            if (savePath.Length == 0) return;
            var sb = new StringBuilder();
            int vertexOffset = 1; // OBJ indices start at 1

            sb.AppendLine($"# Exported from EZGModel: {model.Name}");

            foreach (var mesh in model.Meshes)
            {
                sb.AppendLine($"o {mesh.Name}");

                // Write vertex positions
                foreach (var v in mesh.Vertices)
                {
                    sb.AppendLine($"v {v.Position.X} {v.Position.Y} {v.Position.Z}");
                }

                // Write texture coordinates
                foreach (var v in mesh.Vertices)
                {
                    sb.AppendLine($"vt {v.UV.X} {v.UV.Y}");
                }

                // Write normals
                foreach (var v in mesh.Vertices)
                {
                    sb.AppendLine($"vn {v.Normal.X} {v.Normal.Y} {v.Normal.Z}");
                }

                // Write faces (triangles)
                foreach (var tri in mesh.Triangles)
                {
                    int i1 = mesh.Vertices.IndexOf(tri.vertex1!) + vertexOffset;
                    int i2 = mesh.Vertices.IndexOf(tri.vertex2!) + vertexOffset;
                    int i3 = mesh.Vertices.IndexOf(tri.vertex3!) + vertexOffset;

                    // Face format: f v/vt/vn
                    sb.AppendLine($"f {i1}/{i1}/{i1} {i2}/{i2}/{i2} {i3}/{i3}/{i3}");
                }

                vertexOffset += mesh.Vertices.Count;
            }

            File.WriteAllText(savePath, sb.ToString());
        }

    }
}
