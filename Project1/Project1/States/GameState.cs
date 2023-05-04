﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zone.Models;
using Zone.Managers;
using Zone.Sprites;

namespace Zone.States
{
   public class GameState: State
    {
        Texture2D background;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Sprite _sprites;
        private int[,] map;
        private Box box;
        private List<Box> boxes;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("bg_level1");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };
            _sprites =
                new Sprite(animations)
                {
                    Size = new Point(78, 146),
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
                    Rectangle rect = new Rectangle(x, y, 128, 110);
                    var a = map[i, j];
                    if (a == 1)
                    {
                        box = new Box(_content.Load<Texture2D>("Map/platform"), rect);
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

          
            _sprites.Draw(spriteBatch);
            foreach (var b in boxes)
                b.Draw(spriteBatch);
            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
        
            foreach (var b in boxes)
                if (Collide(_sprites, b))
                    _sprites.Velocity.Y = 0;
            _sprites.Update(gameTime, _sprites);
        }

        protected static bool Collide(Sprite firstObj, Box secondObj)
        {
            Rectangle firstObjRect = new Rectangle((int)firstObj.Position.X,
                (int)firstObj.Position.Y, firstObj.Size.X, firstObj.Size.Y);
            Rectangle secondObjRect = new Rectangle((int)secondObj.Position.X,
                (int)secondObj.Position.Y, secondObj.Position.Width, secondObj.Position.Height);

            return secondObjRect.Intersects(firstObjRect);
        }
    }
}

