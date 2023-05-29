using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level2 : Level
    {
        public Level2(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level2");         
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

            CreateMap(map, "Map/platform");
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var b in boxes)
                if (Collide(player, b)) player.Velocity.Y = 0;

            if (Collide(player, spring) || Collide(player, medal) || Collide(player, flask))
            {
                artifactSoundInstance.Play();
                if (Collide(player, spring)) sprites[spring] = false;
                if (Collide(player, medal)) sprites[medal] = false;
                if (Collide(player, flask)) sprites[flask] = false;
            }

            if (!sprites[spring]) player.isJump = true;
            else player.isJump = false;

            if (!sprites[spring] && !sprites[medal] && !sprites[flask]) _game.ChangeState(new ContinueState(_game, _graphicsDevice, _content, 2));
            if (Collide(player, eye))
            {
                anomalySoundInstance.Play();
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else sprite.Update(gameTime, sprite);
        }
    }
}

