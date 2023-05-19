using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zone.Controls;

namespace Zone.States
{
    public class WinnState : State
    {
        private List<Component> _components;
        Texture2D background;
        public WinnState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("win");

            var quitGameButton = new Button(exitButtonTexture)
            {
                Position = new Vector2(800, 600),
            };
            quitGameButton.Click += QuitGame;

            _components = new List<Component>()
            {
                quitGameButton,
            };
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 2048, 1024), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}

