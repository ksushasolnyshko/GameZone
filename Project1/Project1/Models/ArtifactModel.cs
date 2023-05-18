using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zone.Models
{
    public class ArtifactModel : Sprite
    {
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
