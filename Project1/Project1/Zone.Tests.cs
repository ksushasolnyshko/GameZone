using Xunit;
using Zone.States;

namespace Zone.Tests
{
    public class LevelTests
    {
        private Game1 game;

        public LevelTests()
        {
            game = new Game1();
            game.graphics.ApplyChanges();
        }
        private void Level_test(Level level)
        {
            Assert.NotNull(level);
            Assert.NotNull(level.player);
            Assert.NotNull(level.health);
        }

        private void sprites_test(Level level) => Assert.NotEmpty(level.sprites);

        [Fact]
        private void level1_test()
        {
            var level1 = new Level1(game, game.GraphicsDevice, game.Content);
            Level_test(level1);
            sprites_test(level1);
        }

        [Fact]
        private void level2_test()
        {
            var level2 = new Level2(game, game.GraphicsDevice, game.Content);
            Level_test(level2);
            sprites_test(level2);
        }

        [Fact]
        private void level3_test()
        {
            var level3 = new Level3(game, game.GraphicsDevice, game.Content);
            Level_test(level3);
            sprites_test(level3);
        }

        [Fact]
        private void level4_test()
        {
            var level4 = new Level4(game, game.GraphicsDevice, game.Content);
            Level_test(level4);
            sprites_test(level4);
        }

        [Fact]
        private void level5_test()
        {
            var level5 = new Level5(game, game.GraphicsDevice, game.Content);
            Level_test(level5);
            sprites_test(level5);
        }

        [Fact]
        private void sound_test()
        {
            var level = new Level(game, game.GraphicsDevice, game.Content);
            Assert.NotNull(level.anomalySound);
            Assert.NotNull(level.artifactSound);
        }
    }
}
