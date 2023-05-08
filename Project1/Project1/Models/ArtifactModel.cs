using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone.Models
{
    internal class ArtifactModel : Sprite
    {
        private bool isUp = true;

        public ArtifactModel(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public ArtifactModel(Texture2D texture) : base(texture)
        {
        }

        protected override void SetAnimations()
        {
            if (_animations.ContainsKey("Up")) _animationManager.Play(_animations["Up"]);
        }
    }
}
