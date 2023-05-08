using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone.Models
{
    internal class PlayerModel : Sprite
    {  
        public PlayerModel(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public PlayerModel(Texture2D texture) : base(texture)
        {
            Speed = 8f;
        }
        protected override void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["WalkLeft"]);
            else if (Velocity.Y < 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.Y > 0 && !isFall)
                _animationManager.Play(_animations["WalkLeft"]);
            else _animationManager.Stop();
        }

        public override void Move(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Speed = 6f;
                if (isJump && seconds > _TotalSeconds)
                {
                    _TotalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Velocity.Y = (float)(-Speed * (0.8));
                    Velocity.X = Speed / 2;
                }
                else
                {
                    isJump = false;
                    _TotalSeconds = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Speed = 6f;
                if (isJump && seconds > _TotalSeconds)
                {
                    _TotalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Velocity.Y = (float)(-Speed * (0.8));
                    Velocity.X = -Speed / 2;
                }
                else
                {
                    isJump = false;
                    _TotalSeconds = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X = -Speed;
                isJump = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X = Speed;
                isJump = true;
            }
            
        }
    }
}
