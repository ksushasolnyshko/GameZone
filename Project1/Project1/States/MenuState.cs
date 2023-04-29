﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zone.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

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
                Position = new Vector2(640, 148),
            };
            playButton.Click += PlayButton_Click;

            var rulesButton = new Button(rulesButtomTexture)
            {
                Position = new Vector2(623, 264),
            };
            rulesButton.Click += RulesButton_Click;

            var quitGameButton = new Button(exitButtonTexture)
            {
                Position = new Vector2(630, 384),
            };
            quitGameButton.Click += QuitGameButton_Click;

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
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
        private void RulesButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
