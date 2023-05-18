using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level5 : State
    {
        Texture2D background;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private PlayerModel player;
        private int[,] map;
        private Box box;
        private List<Box> boxes;
        private Sprite health;
        private ArtifactModel crystal;
        private ArtifactModel fly;
        private ArtifactModel flask;
        private ArtifactModel medal;
        private ArtifactModel ball;
        private AnomalyModel brain;
        private AnomalyModel brain2;
        private AnomalyModel eye;
        private AnomalyModel scull;
        private Dictionary<Sprite, bool> sprites;
        private Dictionary<string, Animation> healthAnimation;

        public Level5(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("bg_level5");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
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

            var flaskAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/flask"), 3 )}
            };

            var medalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/medal"), 5 )}
            };

            var scullAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/scull"), 3 )},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/scull"), 3 )}
            };

            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };

            var ballAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/ball"), 3 )}
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

            crystal = new ArtifactModel(crystalAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1800, 560) };

            fly = new ArtifactModel(flyAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1200, 160) };

            brain = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(1800, 790) };

            brain2 = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(900, 150) };

            health = new Sprite(healthAnimation) { Size = new Vector2(285, 72), Position = new Vector2(0, 10) };

            flask = new ArtifactModel(flaskAnimation) { Size = new Vector2(103, 110), Position = new Vector2(960, 390) };

            medal = new ArtifactModel(medalAnimation) { Size = new Vector2(63, 120), Position = new Vector2(30, 260) };

            ball = new ArtifactModel(ballAnimation) { Size = new Vector2(177, 155), Position = new Vector2(1800, 200) };

            eye = new AnomalyModel(eyeAnimation) { Size = new Vector2(98, 62), Position = new Vector2(600, 430) };
            eye.MoveBorder = new Vector2(600, 1400);

            scull = new AnomalyModel(scullAnimation) { Size = new Vector2(73, 100), Position = new Vector2(600, 530) };
            scull.MoveBorder = new Vector2(500, 800);

            sprites = new Dictionary<Sprite, bool>()
            {
                {fly, true},
                {crystal, true},
                {player, true },
                {brain, true},
                {health, true},
                {eye, true},
                {brain2, true},
                {flask, true},
                {medal, true},
                {scull, true},
                {ball, true},
            };

            map = new int[,]
            {{0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0},
             {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0},
             {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
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
            if (Collide(player, brain) || Collide(player, brain2) || Collide(player, scull))
            {
                health.Update(gameTime, health);
            }

            if (Collide(player, eye) || healthAnimation["health"].CurrentFrame >= 6) _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            if (Collide(player, crystal)) sprites[crystal] = false;
            if (Collide(player, fly)) sprites[fly] = false;
            if (Collide(player, medal)) sprites[medal] = false;
            if (Collide(player, flask)) sprites[flask] = false;

            if (!sprites[crystal] && !sprites[fly] && !sprites[medal] && !sprites[flask] && Collide(player, ball)) _game.ChangeState(new Level5(_game, _graphicsDevice, _content));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else if (sprite != health)
                    sprite.Update(gameTime, sprite);
        }
    }
}