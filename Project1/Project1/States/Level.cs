using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level : State
    {
        Texture2D background;
        public PlayerModel player;
        public int[,] map;
        public Box box;
        public List<Box> boxes;
        public ArtifactModel secretBook;
        public ArtifactModel emptyArt;
        public Dictionary<Sprite, bool> sprites;
        public ArtifactModel spring;
        public ArtifactModel flask;
        public ArtifactModel medal;
        public ArtifactModel star;
        public ArtifactModel crystal;
        public ArtifactModel fly;
        public ArtifactModel ball;
        public ArtifactModel heart;
        public AnomalyModel brain;
        public AnomalyModel brain2;
        public AnomalyModel eye;
        public AnomalyModel scull;
        public Dictionary<string, Animation> healthAnimation;
        public Sprite health;


        public Level(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level1");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };

            // Level1
            var emptyAnimation = new Dictionary<string, Animation>
            {
                {"Up", new Animation(_content.Load<Texture2D>("Artifacts/empty2"), 7) }
            };

            var bookAnimation = new Dictionary<string, Animation>
            {
                {"Up", new Animation(_content.Load<Texture2D>("Artifacts/book2"), 5) }
            };

            secretBook = new ArtifactModel(bookAnimation) { Size = new Vector2(57, 61), Position = new Vector2(815, 270) };

            emptyArt = new ArtifactModel(emptyAnimation) { Size = new Vector2(64, 62), Position = new Vector2(20, 165) };

            // Level 2
            var springAnimation = new Dictionary<string, Animation>()
            {
                { "Spring", new Animation(_content.Load<Texture2D>("Artifacts/spring"), 7)},
            };

            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };

            var flaskAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/flask"), 3 )}
            };

            var medalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/medal"), 5 )}
            };

            flask = new ArtifactModel(flaskAnimation) { Size = new Vector2(103, 110), Position = new Vector2(960, 390) };

            medal = new ArtifactModel(medalAnimation) { Size = new Vector2(63, 120), Position = new Vector2(30, 520) };

            spring = new ArtifactModel(springAnimation) { Size = new Vector2(66, 95), Position = new Vector2(1615, 785) };

            eye = new AnomalyModel(eyeAnimation) { Size = new Vector2(98, 62), Position = new Vector2(400, 578) };
            eye.MoveBorder = new Vector2(10, 400);

            // Level3
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

            crystal = new ArtifactModel(crystalAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1800, 170) };

            fly = new ArtifactModel(flyAnimation) { Size = new Vector2(86, 85), Position = new Vector2(500, 170) };

            star = new ArtifactModel(starAnimation) { Size = new Vector2(74, 66), Position = new Vector2(960, 430) };

            brain = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(1800, 790) };

            health = new ArtifactModel(healthAnimation) { Size = new Vector2(285, 72), Position = new Vector2(10, 10) };

            // Level4
            var heartAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/heart"), 3 )}
            };

            heart = new ArtifactModel(heartAnimation) { Size = new Vector2(84, 68), Position = new Vector2(120, 300) };
            brain2 = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(900, 150) };

            // Level5

            var scullAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/scull"), 3 )},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/scull"), 3 )}
            };

            scull = new AnomalyModel(scullAnimation) { Size = new Vector2(73, 100), Position = new Vector2(600, 530) };
            scull.MoveBorder = new Vector2(500, 800);

            var ballAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/ball"), 3 )}
            };
            ball = new ArtifactModel(ballAnimation) { Size = new Vector2(177, 155), Position = new Vector2(1800, 200) };

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

            map = new int[,]
            {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            CreateMap(map, "Map/platform");   
        }

        public void SetBackGround(string path)
        {
            background = _content.Load<Texture2D>(path);
        }

        public void CreateMap(int[,] map, string path)
        {
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
                        box = new Box(_content.Load<Texture2D>(path))
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
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else sprite.Update(gameTime, sprite);
        }
    }
}
