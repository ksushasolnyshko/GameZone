using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Zone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zone.Models;

namespace Zone.States
{
    public abstract class State
    {
        #region Fields

        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected Game1 _game;

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);


        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;

            _graphicsDevice = graphicsDevice;

            _content = content;
        }

        public abstract void Update(GameTime gameTime);

        protected static bool Collide(Sprite firstObj, Sprite secondObj)
        {
            Rectangle firstObjRect = new Rectangle((int)firstObj.Position.X,
                (int)firstObj.Position.Y, (int)firstObj.Size.X, (int)firstObj.Size.Y);
            Rectangle secondObjRect = new Rectangle((int)secondObj.Position.X,
                (int)secondObj.Position.Y, (int)secondObj.Size.X, (int)secondObj.Size.Y);
            return firstObjRect.Intersects(secondObjRect);
        }

        public void QuitGame(object sender, EventArgs e)
        {
            _game.Exit();
        }
        #endregion
    }
}
