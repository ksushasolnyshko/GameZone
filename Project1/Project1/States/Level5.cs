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
    public class Level5 : Level
    {
        Texture2D background;

        public Level5(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            SetBackGround("bg_level5");

            var crystalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/crystal"), 5 )}
            };

            var flyAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/fly"), 2 )}
            };

            var flaskAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/flask"), 3 )}
            };

            var medalAnimation = new Dictionary<string, Animation>()
            {
                { "Up", new Animation(_content.Load<Texture2D>("Artifacts/medal"), 5 )}
            };
         
            var eyeAnimation = new Dictionary<string, Animation>()
            {
                { "goright", new Animation(_content.Load<Texture2D>("Anomalyes/eyeRight"), 3)},
                { "goleft", new Animation(_content.Load<Texture2D>("Anomalyes/eyeLeft"), 3)}
            };

            crystal = new ArtifactModel(crystalAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1800, 560) };

            fly = new ArtifactModel(flyAnimation) { Size = new Vector2(86, 85), Position = new Vector2(1200, 160) };

            brain = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(1800, 790) };

            brain2 = new AnomalyModel(_content.Load<Texture2D>("Anomalyes/brain")) { Size = new Vector2(60, 90), Position = new Vector2(900, 150) };

            flask = new ArtifactModel(flaskAnimation) { Size = new Vector2(103, 110), Position = new Vector2(960, 390) };

            medal = new ArtifactModel(medalAnimation) { Size = new Vector2(63, 120), Position = new Vector2(30, 260) };

            eye = new AnomalyModel(eyeAnimation) { Size = new Vector2(98, 62), Position = new Vector2(600, 430) };
            eye.MoveBorder = new Vector2(600, 1400);


            sprites = new Dictionary<Sprite, bool>()
            {
                {fly, true},
                {crystal, true},
                {player, true },
                {brain, true},
                {health, true},
                {eye, true},
                {brain2, true},
                {flask, true},
                {medal, true},
                {scull, true},
                {ball, true},
            };

            map = new int[,]
            {{0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
             {0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0},
             {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
             {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0},
             {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
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
            if (Collide(player, brain) || Collide(player, brain2) || Collide(player, scull))
            {
                health.Update(gameTime, health);
            }

            if (Collide(player, eye) || healthAnimation["health"].CurrentFrame >= 6) _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
            if (Collide(player, crystal)) sprites[crystal] = false;
            if (Collide(player, fly)) sprites[fly] = false;
            if (Collide(player, medal)) sprites[medal] = false;
            if (Collide(player, flask)) sprites[flask] = false;

            if (!sprites[crystal] && !sprites[fly] && !sprites[medal] && !sprites[flask] && Collide(player, ball)) _game.ChangeState(new Level5(_game, _graphicsDevice, _content));
            foreach (var sprite in sprites.Keys)
                if (sprite == player) sprite.Update(gameTime, sprite, boxes);
                else if (sprite != health)
                    sprite.Update(gameTime, sprite);
        }
    }
}