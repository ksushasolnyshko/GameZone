using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level3 : Level
    {
        public Level3(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level3");

            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };
          

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

            CreateMap(map, "Map/snow_platform");
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var b in boxes)
                if (Collide(player, b)) player.Velocity.Y = 0;
            player.isJump = true;
            if (Collide(player, brain))
            {
                anomalySoundInstance.Play();
                health.Update(gameTime, health);
            }

            if (Collide(player, eye) || healthAnimation["health"].CurrentFrame >= 6)
            {   
                if (Collide(player, eye)) anomalySoundInstance.Play();
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }
            foreach (var art in new List<ArtifactModel> { crystal, star, fly })
            {
                CheckCollision(art);
            }

            if (!sprites[crystal] && !sprites[star] && !sprites[fly]) _game.ChangeState(new ContinueState(_game, _graphicsDevice, _content, 3));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else if (sprite != health)
                    sprite.Update(gameTime, sprite);
        }
    }
}