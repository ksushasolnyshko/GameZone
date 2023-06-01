using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Zone.Controls;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using System.Linq;

namespace Zone.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        Texture2D background;
        public SoundEffect levelSound;
        public SoundEffectInstance levelsSoundInstance;
        public int levelNumber;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var playButtomTexture = _content.Load<Texture2D>("Controls/play_button");
            var rulesButtomTexture = _content.Load<Texture2D>("Controls/rules_button");
            var exitButtonTexture = _content.Load<Texture2D>("Controls/exit_button");
            background = _content.Load<Texture2D>("background");
            levelSound = _content.Load<SoundEffect>("Sounds/level_sound");
            levelsSoundInstance = levelSound.CreateInstance();
            levelsSoundInstance.IsLooped = true;
            levelsSoundInstance.Volume = 0.6f;

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
        public async void LoadLevelNumber()
        {
            await readXMLAsync();
        }

        private async Task readXMLAsync()
        {
            var serializer = new DataContractSerializer(typeof(Int16));

            bool existed = await FileExists(ApplicationData.Current.LocalFolder, "level.xml");
            if (existed)
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("level.xml"))
                {
                    levelNumber = (Int16)serializer.ReadObject(stream);
                }
            }
            else levelNumber = 0;
        }


        public async Task<bool> FileExists(StorageFolder folder, string fileName)
        {
            return (await folder.GetFilesAsync()).Any(x => x.Name == fileName);
        }


        private void PlayButton_Click(object sender, EventArgs e)
        {
            LoadLevelNumber();
            if (levelNumber != 0)
            {
                if (levelNumber == 1) _game.ChangeState(new Level2(_game, _graphicsDevice, _content));
                else if (levelNumber == 2) _game.ChangeState(new Level3(_game, _graphicsDevice, _content));
                else if (levelNumber == 3) _game.ChangeState(new Level4(_game, _graphicsDevice, _content));
                else _game.ChangeState(new Level5(_game, _graphicsDevice, _content));
            }
            else _game.ChangeState(new Level4(_game, _graphicsDevice, _content));
        }
        private void RulesButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new RulesState(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            levelsSoundInstance.Play();
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
