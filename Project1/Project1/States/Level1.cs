using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level1 : State
    {
        Texture2D background;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private PlayerModel player;
        private ArtifactModel secretBook;
        private ArtifactModel emptyArt;
        private int[,] map;
        private Box box;
        private List<Box> boxes;
        private bool isBook = true;
        private bool isEmpty = true;

        public Level1(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("bg_level1");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };

            var emptyAnimation = new Dictionary<string, Animation>
            {
                {"Up", new Animation(_content.Load<Texture2D>("Artifacts/empty2"), 7) }
            };

            var bookAnimation = new Dictionary<string, Animation>
            {
                {"Up", new Animation(_content.Load<Texture2D>("Artifacts/book2"), 5) }
            };

            secretBook = new ArtifactModel(bookAnimation)
            {
                Size = new Vector2(57, 61),
                Position = new Vector2(815, 270)
            };

            emptyArt = new ArtifactModel(emptyAnimation)
            {
                Size = new Vector2(64, 62),
                Position = new Vector2(20, 165)
            };

            player =
                new PlayerModel(animations)
                {
                    Size = new Vector2(78, 144),
                    Position = new Vector2(100, 750),
                    Input = new Input()
                    {
                        Right = Keys.D,
                        Left = Keys.A,
                        Up = Keys.W,
                    }
                };

            map = new int[,]
            {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
             {1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1},
             {0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            var x = 0;
            var y = 0;
            boxes = new List<Box>();
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    var a = map[i, j];
                    if (a == 1)
                    {
                        box = new Box(_content.Load<Texture2D>("Map/platform"))
                        {
                            Size = new Vector2(116, 97),
                            Position = new Vector2(x, y),
                        };
                        boxes.Add(box);
                    }
                    x += 117;
                }
                x = 0;
                y += 128;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 2048, 1024), Color.White);
            player.Draw(spriteBatch);
            if (isBook) secretBook.Draw(spriteBatch);
            if (isEmpty) emptyArt.Draw(spriteBatch);
            foreach (var b in boxes)
                b.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var b in boxes)
                if (Collide(player, b))
                {
                    if (player.Velocity.Y >= b.Velocity.Y + b.Size.Y)
                        player.Velocity.Y = -5;
                    else player.Velocity.Y = 0;
                }

            if (Collide(player, secretBook)) isBook = false;
            if (Collide(player, emptyArt)) isEmpty = false;
            if (!isEmpty && !isBook) _game.ChangeState(new Level2(_game, _graphicsDevice, _content));
            player.Update(gameTime, player);
            emptyArt.Update(gameTime, emptyArt);
            secretBook.Update(gameTime, secretBook);
        }
    }
}

