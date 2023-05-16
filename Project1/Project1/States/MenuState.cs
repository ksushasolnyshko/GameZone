using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zone.Controls;

namespace Zone.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        Texture2D background;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var playButtomTexture = _content.Load<Texture2D>("Controls/play_button");
            var rulesButtomTexture = _content.Load<Texture2D>("Controls/rules_button");
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("background");

            var playButton = new Button(playButtomTexture)
            {
                Position = new Vector2(1300, 350),
            };
            playButton.Click += PlayButton_Click;

            var rulesButton = new Button(rulesButtomTexture)
            {
                Position = new Vector2(1290, 500),
            };
            rulesButton.Click += RulesButton_Click;

            var quitGameButton = new Button(exitButtonTexture)
            {
                Position = new Vector2(1300, 665),
            };
            quitGameButton.Click += QuitGame;

            _components = new List<Component>()
            {
                playButton,
                rulesButton,
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

        private void PlayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new Level4(_game, _graphicsDevice, _content));
        }
        private void RulesButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new Level1(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
