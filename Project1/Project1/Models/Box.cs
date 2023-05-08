using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zone.Models
{
    public class Box : Sprite
    {
        public Box(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public Box(Texture2D texture) : base(texture)
        {
        }
    }
}
