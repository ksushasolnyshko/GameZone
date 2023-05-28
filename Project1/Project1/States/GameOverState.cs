using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zone.Controls;

namespace Zone.States
{
    public class GameOverState : State
    {
        private List<Component> _components;
        Texture2D background;
        public SoundEffect gameOverSound;
        public SoundEffectInstance gameOverInstance;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {   
            var playButtomTexture = _content.Load<Texture2D>("Controls/replay_button");
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("GameOverBackground");

            gameOverSound = _content.Load<SoundEffect>("Sounds/gameOver_sound");
            gameOverInstance = gameOverSound.CreateInstance();
            gameOverInstance.IsLooped = false;

            var replayButton = new Button(playButtomTexture)
            {
                Position = new Vector2(300, 665),
            };
            replayButton.Click += ReplayButton_Click;

            var quitGameButton = new Button(exitButtonTexture)
            {
                Position = new Vector2(1300, 665),
            };
            quitGameButton.Click += QuitGame;

            _components = new List<Component>()
            {
                replayButton,
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
        private void ReplayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new Level1(_game, _graphicsDevice, _content));
            gameOverInstance.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            gameOverInstance.Play();
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
