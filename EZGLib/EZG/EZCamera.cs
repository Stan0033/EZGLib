using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EZGLib.EZG
{
    public class EZCamera
    {
        public Vector3 Position, Target, Roll;
        public EZTransformation? APosition, ATarget, ARoll;
        public string Name = "Unnamed Camera";
        public float FOV, Near, Far = 0;
    }
}
