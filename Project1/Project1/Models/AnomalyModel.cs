using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Zone.Models
{
    internal class AnomalyModel : Sprite
    {
        private bool isGoLeft = true;
        public Vector2 MoveBorder = new Vector2(5, 1940);

        public AnomalyModel(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 2f;
        }

        public AnomalyModel(Texture2D texture) : base(texture)
        {
            Speed = 3f;
        }

        protected override void SetAnimations()
        {
            if (_animationManager != null)
                if (!isGoLeft) _animationManager.Play(_animations["goright"]);
                else _animationManager.Play(_animations["goleft"]);
        }

        public override void Move(GameTime gameTime)
        {
            if (Position.X < MoveBorder.X) isGoLeft = false;
            else if (Position.X > MoveBorder.Y) isGoLeft = true;
            if (isGoLeft) Velocity.X = -Speed;
            else Velocity.X = Speed;
        }
    }
}
