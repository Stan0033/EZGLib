using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
 

namespace EZGLib.EZG
{
    public enum InterpolationType
    {
        None, Linear, Bezier, Hermite
    }
    public class EZTransformation
    {
        public bool IsStatic = true;
        public Vector3 StaticValue = new ();
        public Dictionary<int, Vector3> Keyframes = new Dictionary<int, Vector3>();
        public EZModel? Owner;
        public InterpolationType Type = InterpolationType.None;
        public void Animate()
        {
            IsStatic = false;
            Keyframes.Clear();
            Keyframes.Add(0, new Vector3(StaticValue.X, StaticValue.Y, StaticValue.Z));
        }
        public EZTransformation(EZModel owner)
        {
            this.Owner = owner;
        }
        public override string ToString()
        {
           throw new NotImplementedException();
        }
    }


}
