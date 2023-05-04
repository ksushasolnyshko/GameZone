using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zone.Models
{
    public class Box
    {
        Texture2D Texture;
        public Rectangle Position;

        public Box(Texture2D texture, Rectangle rect)
        {
            this.Texture = texture;
            this.Position = rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
