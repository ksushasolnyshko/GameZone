using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level2 : State
    {
        Texture2D background;
        private PlayerModel player;
        private ArtifactModel spring;
        private ArtifactModel flask;
        private ArtifactModel medal;
        private AnomalyModel eye;
        private int[,] map;
        private Box box;
        private List<Box> boxes;
        private Dictionary<Sprite, bool> sprites;

        public Level2(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("bg_level2");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };

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

            flask = new ArtifactModel(flaskAnimation){Size = new Vector2(103, 110), Position = new Vector2(960, 390)};

            medal = new ArtifactModel(medalAnimation){Size = new Vector2(63, 120), Position = new Vector2(30, 520)};

            spring = new ArtifactModel(springAnimation){Size = new Vector2(66, 95), Position = new Vector2(1615, 785)};

            eye = new AnomalyModel(eyeAnimation){Size = new Vector2(98, 62), Position = new Vector2(400, 578)};
            eye.MoveBorder = new Vector2(10, 400);

            player =
                new PlayerModel(animations)
                {
                    Size = new Vector2(56, 144),
                    Position = new Vector2(100, 745),
                    Input = new Input()
                    {
                        Right = Keys.D,
                        Left = Keys.A,
                        Up = Keys.W,
                    }
                };

            sprites = new Dictionary<Sprite, bool>()
            {
                {spring, true},
                {medal, true},
                {flask, true},
                {player, true},
                {eye, true}
            };

            map = new int[,]
            {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1},
             {1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
             {0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0},
             {1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
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

            if (Collide(player, spring)) sprites[spring] = false;
            if (Collide(player, medal)) sprites[medal] = false;
            if (Collide(player, flask)) sprites[flask] = false;

            if (!sprites[spring]) player.isJump = true;
            else player.isJump = false;

            if (!sprites[spring] && !sprites[medal] && !sprites[flask]) _game.ChangeState(new Level3(_game, _graphicsDevice, _content));
            if (Collide(player, eye))
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else sprite.Update(gameTime, sprite);
        }
    }
}
