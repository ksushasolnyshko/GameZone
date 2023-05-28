using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level1 : Level
    {
        public Level1(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level1");
            var animations = new Dictionary<string, Animation>(){
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/player_go_left"), 8) },
                {"WalkUp", new Animation(_content.Load<Texture2D>("Player/player_go_right"), 8) },
            };


            sprites = new Dictionary<Sprite, bool>()
            {
                {emptyArt, true},
                {secretBook, true},
                {player, true},
            };

            map = new int[,]
            {{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1},
             {0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            CreateMap(map, "Map/platform");
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var b in boxes)
                if (Collide(player, b)) player.Velocity.Y = 0;
            player.isJump = true;
            if (Collide(player, secretBook) || Collide(player, emptyArt))
            {
                artifactSoundInstance.Play();
                if (Collide(player, emptyArt)) sprites[emptyArt] = false;
                if (Collide(player, secretBook)) sprites[secretBook] = false;
            }

            if (!sprites[secretBook] && !sprites[emptyArt]) _game.ChangeState(new Level2(_game, _graphicsDevice, _content));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else sprite.Update(gameTime, sprite);
        }
    }
}

