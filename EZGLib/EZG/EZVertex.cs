using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
 

namespace EZGLib.EZG
{
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
  

}
