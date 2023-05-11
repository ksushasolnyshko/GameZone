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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Sprite healthForm;
        private PlayerModel player;
        private ArtifactModel spring;
        private AnomalyModel eye;
        private int[,] map;
        private Box box;
        private List<Box> boxes;
        private bool isSpring = false;
        private int playerHealth = 7;

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

            var healthAnimation = new Dictionary<string, Animation>()
            {
                {"health", new Animation(_content.Load<Texture2D>("health"), 7)}
            };

            healthForm = new Sprite(healthAnimation)
            {
                Size = new Vector2(285, 72),
                Position = new Vector2(10, 10)
            };

            spring = new ArtifactModel(springAnimation)
            {
                Size = new Vector2(66, 95),
                Position = new Vector2(1615, 785)
            };

            eye = new AnomalyModel(eyeAnimation)
            {
                Size = new Vector2(95, 62),
                Position = new Vector2(400, 156)
            };

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
            eye.Draw(spriteBatch);
            healthForm.Draw(spriteBatch);
            if (!isSpring) spring.Draw(spriteBatch);
            foreach (var b in boxes)
                b.Draw(spriteBatch);
            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {

            foreach (var b in boxes)
                if (Collide(player, b)) player.Velocity.Y = 0;
                
            if (Collide(player, spring)) isSpring = true;
            if (isSpring) player.isJump = true;
            else player.isJump = false;
            if (Collide(player, eye))
            {
                playerHealth -= 1;
                healthForm.Update(gameTime, healthForm);
                player.Velocity.X = 20 * player.Speed;
            }
           
            player.Update(gameTime, player, boxes);
            spring.Update(gameTime, spring);
            eye.Update(gameTime, eye);
        }

        protected static bool Collide(Sprite firstObj, Sprite secondObj)
        {
            Rectangle firstObjRect = new Rectangle((int)firstObj.Position.X,
                (int)firstObj.Position.Y, (int)firstObj.Size.X, (int)firstObj.Size.Y);
            Rectangle secondObjRect = new Rectangle((int)secondObj.Position.X,
                (int)secondObj.Position.Y, (int)secondObj.Size.X, (int)secondObj.Size.Y);

            return firstObjRect.Intersects(secondObjRect);
        }
    }
}
