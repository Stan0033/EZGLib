using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EZGLib.EZG
{
    public class EZMaterial
    {
        public string Name = "DefaultMaterial";

        // RGB colors as Vector3 (R, G, B), values from 0 to 1
        public Vector3 DiffuseColor = new Vector3(1, 1, 1);
        public Vector3 AmbientColor = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 SpecularColor = new Vector3(1, 1, 1);
        public bool TwoSided = false;
        public bool Unshaded = false;

        // Shininess (specular exponent)
        public float Shininess = 32f;

        // Transparency (0 = fully transparent, 1 = fully opaque)
        public float Opacity = 1f;

        // Optional diffuse texture filename (relative or absolute)
        public string DiffuseTexture = string.Empty;
        public EZModel? Owner;

        public EZMaterial(string name, EZModel owner)
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

}
