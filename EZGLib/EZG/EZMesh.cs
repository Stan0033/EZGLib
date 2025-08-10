using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZGLib.EZG
{

    public class EZMesh
    {
        public List<EZVertex> Vertices = new List<EZVertex>();
        public List<EZTriangle> Triangles = new List<EZTriangle>();
        public List<EZEdge> Edges = new List<EZEdge>();
        public EZMaterial? Material;
        public EZTransformation? Opacity, Color; 
        public string Name = "Unnamed";

    }

}
