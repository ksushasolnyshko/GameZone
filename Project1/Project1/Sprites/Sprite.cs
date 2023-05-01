using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zone.Models;
using Zone.Managers;

namespace Zone.Sprites
{
    public class Sprite
    {
        #region Fields

        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;
        protected Vector2 _position;
        protected Vector2 current_position;
        protected Texture2D _texture;

        KeyboardState state;
        KeyboardState Oldstate = Keyboard.GetState();
        bool isJump = false;
        float _TotalSeconds = 0;
        float seconds = 0.3f;

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;

        public Vector2 Velocity;

        #endregion

        #region Methods

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("This ain't right..!");
        }

        public virtual void Move(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Input.Up))
            //    Velocity.Y = -Speed;
            //else if (Keyboard.GetState().IsKeyDown(Input.Down))
            //    Velocity.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Up) && Oldstate.IsKeyDown(Keys.W))
            {
                isJump = true;
            }
            if (isJump && seconds > _TotalSeconds)
            {
                _TotalSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity.Y = -7;
                Velocity.X = Speed;
            }
            else
            {
                if (Position.Y < 30)
                {  
                    Velocity.Y += 7;
                    _TotalSeconds = 0;//Если герой упал, обнуляем счетчик.
                }
                isJump = false;
            }
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["WalkLeft"]);
            //else if (Velocity.Y > 0)
            //    _animationManager.Play(_animations["WalkDown"]);
            else if (Velocity.Y < 0)
               _animationManager.Play(_animations["WalkRight"]);
            else _animationManager.Stop();
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            state = Keyboard.GetState();
            Oldstate = state;
            Move(gameTime);

            SetAnimations();

            _animationManager.Update(gameTime);
            current_position = Position;
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion
    }
}
