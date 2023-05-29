using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System.Collections.Generic;
using Zone.Managers;
using Zone.Models;

namespace Zone.States
{
    public class Level4 : Level
    {
        public Level4(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level4");

            var starAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/star"), 3 )}
            };

            var crystalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/crystal"), 5 )}
            };

            var flyAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/fly"), 2 )}
            };

            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };

            crystal = new ArtifactModel(crystalAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1800, 170) };

            fly = new ArtifactModel(flyAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1200, 160) };

            star = new ArtifactModel(starAnimation) { Size = new Vector2(74, 66), Position = new Vector2(1100, 425) };

            brain = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(1800, 790) };

            eye = new AnomalyModel(eyeAnimation) { Size = new Vector2(98, 62), Position = new Vector2(600, 430) };
            eye.MoveBorder = new Vector2(600, 1700);

            sprites = new Dictionary<Sprite, bool>()
            {
                {fly, true},
                {star, true},
                {crystal, true},
                {player, true },
                {brain, true},
                {health, true},
                {eye, true},
                {heart, true},
                {brain2, true }
            };

            map = new int[,]
            {{0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1},
             {1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0},
             {0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
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
            if (Collide(player, brain) || Collide(player, brain2))
            {
                anomalySoundInstance.Play();
                health.Update(gameTime, health);
            }

            if (Collide(player, eye) || healthAnimation["health"].CurrentFrame >= 6)
            {
                if (Collide(player, eye)) anomalySoundInstance.Play();
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            }
            if (Collide(player, crystal) || Collide(player, star) || Collide(player, fly) || Collide(player, heart))
                artifactSoundInstance.Play();
            if (Collide(player, crystal)) sprites[crystal] = false;
            if (Collide(player, star)) sprites[star] = false;
            if (Collide(player, fly)) sprites[fly] = false;
            if (Collide(player, heart)) sprites[heart] = false;

            if (!sprites[crystal] && !sprites[star] && !sprites[fly] && !sprites[heart]) _game.ChangeState(new ContinueState(_game, _graphicsDevice, _content, 4));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else if (sprite != health)
                    sprite.Update(gameTime, sprite);
        }
    }
}