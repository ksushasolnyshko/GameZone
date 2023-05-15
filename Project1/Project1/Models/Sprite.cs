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

namespace Zone.Models
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
        public bool isJump = false;
        public bool isFall = false;
        public bool isPlayer = false;
        public float _TotalSeconds = 0;
        public float seconds = 0.8f;
        #endregion

        #region Properties
        public Vector2 Size;
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

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public float Speed = 0f;

        public Vector2 Velocity = new Vector2(0, 0);

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



        protected virtual void SetAnimations() 
        {
            _animationManager.Play(_animations[_animations.Keys.First()]);
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

        public virtual void Move(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime, Sprite sprites, List<Box> platforms = null)
        {
            state = Keyboard.GetState();
            Move(gameTime);

            SetAnimations();

            if (_animationManager != null) _animationManager.Update(gameTime);
            current_position = Position;
            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion
    }
}