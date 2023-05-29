using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Zone.Controls;

namespace Zone.States
{
    public class ContinueState : State
    {
        private List<Component> _components;
        Texture2D background;
        private int levelNumber;

        public ContinueState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, int levelNum)
          : base(game, graphicsDevice, content)
        {
            var playButtomTexture = _content.Load<Texture2D>("Controls/play_button");
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("continue_bg");

            var playButton = new Button(playButtomTexture)
            {
                Position = new Vector2(300, 665),
            };
            playButton.Click += PlayButton_Click;

            var quitGameButton = new Button(exitButtonTexture)
            {
                Position = new Vector2(1300, 665),
            };
            quitGameButton.Click += QuitGame;

            _components = new List<Component>()
            {
                playButton,
                quitGameButton,
            };

            levelNumber = levelNum;
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
            if (levelNumber == 1) _game.ChangeState(new Level2(_game, _graphicsDevice, _content));
            else if (levelNumber == 2) _game.ChangeState(new Level3(_game, _graphicsDevice, _content));
            else if (levelNumber == 3) _game.ChangeState(new Level4(_game, _graphicsDevice, _content));
            else _game.ChangeState(new Level5(_game, _graphicsDevice, _content));
        }

        public async void SaveLevelNumber()
        {
            await writeXMLAsync();
        }

        private async Task writeXMLAsync()
        {
            var serializer = new DataContractSerializer(typeof(Int16));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                 "level.xml", CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, levelNumber);
            }
        }

        public override void QuitGame(object sender, EventArgs e)
        { 
            SaveLevelNumber();
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}