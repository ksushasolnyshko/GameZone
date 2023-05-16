using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level3 : State
    {
        Texture2D background;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private PlayerModel player;
        private int[,] map;
        private Box box;
        private List<Box> boxes;
        private Sprite health;
        private ArtifactModel star;
        private ArtifactModel crystal;
        private ArtifactModel fly;
        private AnomalyModel brain;
        private AnomalyModel eye;
        private Dictionary<Sprite, bool> sprites;
        private Dictionary<string, Animation> healthAnimation;

        public Level3(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("bg_level3");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };

            var starAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/star"), 3 )}
            };

            var crystalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/crystal"), 5 )}
            };

            var flyAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/fly"), 2 )}
            };

            healthAnimation = new Dictionary<string, Animation>()
            {
                { "health", new Animation(_content.Load<Texture2D>("health"), 7 )}
            };

            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };

            player =
                new PlayerModel(animations)
                {
                    Size = new Vector2(56, 144),
                    Position = new Vector2(100, 750),
                    Input = new Input()
                    {
                        Right = Keys.D,
                        Left = Keys.A,
                        Up = Keys.W,
                    }
                };

            crystal = new ArtifactModel(crystalAnimation){Size = new Vector2(86, 85), Position = new Vector2(1800, 170)};

            fly = new ArtifactModel(flyAnimation){Size = new Vector2(86, 85), Position = new Vector2(500, 170)};

            star = new ArtifactModel(starAnimation){Size = new Vector2(74, 66), Position = new Vector2(960, 430)};

            brain = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")){Size = new Vector2(60, 90), Position = new Vector2(1800, 790)};

            health = new ArtifactModel(healthAnimation){Size = new Vector2(285, 72), Position = new Vector2(10, 10)};

            eye = new AnomalyModel(eyeAnimation) { Size = new Vector2(98, 62), Position = new Vector2(400, 170) };
            eye.MoveBorder = new Vector2(250, 600);

            sprites = new Dictionary<Sprite, bool>()
            {
                {fly, true},
                {star, true},
                {crystal, true},
                {player, true },
                {brain, true},
                {health, true},
                {eye, true}
            };

            map = new int[,]
            {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0},
             {0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
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
                        box = new Box(_content.Load<Texture2D>("Map/snow_platform"))
                        {
                            Size = new Vector2(117, 97),
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
            foreach (var sprite in sprites.Keys)
                if (sprites[sprite]) sprite.Draw(spriteBatch);
            foreach (var b in boxes)
                b.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var b in boxes)
                if (Collide(player, b)) player.Velocity.Y = 0;
            player.isJump = true;
            if (Collide(player, brain))
            {
                health.Update(gameTime, health);
            }

            if (Collide(player, eye) || healthAnimation["health"].CurrentFrame >= 6)  _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            if (Collide(player, crystal)) sprites[crystal] = false;
            if (Collide(player, star)) sprites[star] = false;
            if (Collide(player, fly)) sprites[fly] = false;
            
            if (!sprites[crystal] && !sprites[star] && !sprites[fly]) _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else if (sprite != health)
                    sprite.Update(gameTime, sprite);
        }
    }
}