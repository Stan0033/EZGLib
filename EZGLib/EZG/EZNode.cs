using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EZGLib.EZG
{
    public class EZNode
        {
            public string Name = string.Empty;
            public Vector3 PivotPoint = new Vector3();
            public EZNode? Parent;
            public EZTransformation? Translation, Rotation, Scaling;
            public EZModel Owner;
            public bool InheritsTranslation, InheritsScaling,InheritsRotation;
            public EZNode(string name, EZModel owner)
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
      
        public class EZCOllision : EZNode
        {
            public Vector3 MinimumExtent, MaximumExtent;

            public EZCOllision(string name, EZModel owner) : base(name, owner)
            {
            }
        }
        public enum LightType
        {
            Omnidirectional,Directional,Ambient
        }
        public class EZLight : EZNode
        {
            public LightType LightType = LightType.Omnidirectional;

            public EZLight(string name, EZModel owner) : base(name, owner)
            {
            }
        }
}
