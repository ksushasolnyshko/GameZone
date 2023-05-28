using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public SoundEffect winnSound;
        public SoundEffectInstance winnInstance;
        public WinnState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("win");

            winnSound = _content.Load<SoundEffect>("Sounds/win_sound");
            winnInstance = winnSound.CreateInstance();
            winnInstance.IsLooped = false;

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
            winnSound.Play();
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}

