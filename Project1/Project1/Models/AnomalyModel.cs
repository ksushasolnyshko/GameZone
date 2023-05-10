using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone.Models
{
    internal class AnomalyModel : Sprite
    {
        private bool isGoLeft = true;
   
        public AnomalyModel(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 1f;
        }

        public AnomalyModel(Texture2D texture) : base(texture)
        {
        }

        protected override void SetAnimations()
        {
            if (!isGoLeft) _animationManager.Play(_animations["goright"]);
            else _animationManager.Play(_animations["goleft"]);
        }

        public override void Move(GameTime gameTime)
        {
            if (Position.X < 10) isGoLeft = false;
            else if (Position.X > 600) isGoLeft = true;
            if (isGoLeft) Velocity.X = -Speed;
            else Velocity.X = Speed;
        }
    }
}
