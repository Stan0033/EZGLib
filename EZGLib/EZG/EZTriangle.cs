using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace EZGLib.EZG
{
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
        public string GetString()
        {
            return $"{Owner.Vertices.IndexOf(vertex1)} {Owner.Vertices.IndexOf(vertex2)} {Owner.Vertices.IndexOf(vertex3)}";
        }
    }
}
