Very Simple Geometry (VSG) is a lightweight, human-readable file format designed for compact and efficient storage of 3D mesh data. 



I made this, inspired by other simple 3d storing formats like OBJ and STL.

It's extremely compact and human-readable — a whole mesh is a single line:

<vertex_data|triangle_indices|color>

Example:
<0 0 0 0 1 0 0 0 1 0 0 0 1 1 0 1 0 0 0 1 0 0 1 1|0 1 2|255 255 255>

Below is an example in C# demonstrating how to load and save a model using the EZGLib implementation of VSG:

 
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

string savePath = VerySimpleGeometry.SaveVsgOrObjFile();
if (savePath.Length > 0)
{
    if (Path.GetExtension(savePath).ToLower() == ".vsg")
    {
        model.Save(savePath);
    }
    else if (Path.GetExtension(savePath).ToLower() == ".obj")
    {
        model.WriteOBJ(savePath);
    }
}
