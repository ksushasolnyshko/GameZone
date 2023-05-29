using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;

namespace Zone.Models
{
    public class PlayerModel : Sprite
    {
        public int health = 7;
        private bool isUp = true;
        private bool isRight = true;
        private bool isLeft = true;

        public PlayerModel(Dictionary<string, Animation> animations) : base(animations)
        {
            isPlayer = true;
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
            if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Right) && isUp && isRight)
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
            else if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Left) && isUp && isLeft)
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
            else if (Keyboard.GetState().IsKeyDown(Input.Left) && isLeft)
            {
                Velocity.X = -Speed;
                isJump = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Input.Right) && isRight)
            {
                Velocity.X = Speed;
                isJump = true;
            }
        }

        #region Colloision
        public bool IsTouchingLeft(Sprite sprite)
        {
            return Rectangle.Right + Velocity.X > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Left &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingRight(Sprite sprite)
        {
            return Rectangle.Left + Velocity.X < sprite.Rectangle.Right &&
              Rectangle.Right > sprite.Rectangle.Right &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingTop(Sprite sprite)
        {
            return Rectangle.Bottom + Velocity.Y > Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Top &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        public bool IsTouchingBottom(Sprite sprite)
        {
            return Rectangle.Top + Velocity.Y < sprite.Rectangle.Bottom &&
              Rectangle.Bottom > sprite.Rectangle.Bottom &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        #endregion
        public override void Update(GameTime gameTime, Sprite sprites, List<Box> platforms = null)
        {
            Move(gameTime);
            if (platforms != null)
                foreach (var b in platforms)
                {
                    if ((Velocity.X > 0 && IsTouchingLeft(b)) || (Velocity.X < 0 & IsTouchingRight(b)))
                        if (Velocity.Y != 0) Velocity.X = 0;
                    if (Velocity.Y < 0 & IsTouchingBottom(b)) Velocity.Y = 0;
                }
            SetAnimations();
            if (_animationManager != null) _animationManager.Update(gameTime);
            current_position = Position;
            Position += Velocity;
            Velocity = Vector2.Zero;
            if (Position.Y <= 10)
            {
                Velocity.Y = 0;
                isUp = false;
            }
            else isUp = true;
            if(Position.X >= 1930)
            {
                Velocity.X = 5;
                isRight = false;
            }
            else isRight = true;

            if (Position.X <= 5)
            {
                Velocity.X = 0;
                isLeft= false;
            }
            else isLeft = true;

            if (Position.Y < 745)
            {
                isFall = true;
                Velocity.Y += 5;
            }
            else isFall = false;
        }
    }
}
