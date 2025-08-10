using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace EZGLib.EZG
{
    public class EZModel
    {

        public List<EZMesh> Meshes = new List<EZMesh>();
        public List<EZMaterial> Materials = new List<EZMaterial>();
        public List<EZTransformation> Transformations = new List<EZTransformation>();
        public List<EZNode> Nodes = new List<EZNode>();
        public List<EZSequence> Sequences = new List<EZSequence>();
        public List<EZTexture> Textures = new List<EZTexture>();
        public List<EZTextureAnimation> TextureAnimations = new List<EZTextureAnimation>();
        public List<EZCamera> Cameras = new List<EZCamera>();
        public string Name = "Unnamed";


    }
}
