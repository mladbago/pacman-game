using PacMan;

namespace TestPacMan
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GameStarted()
        {
            MainWindow mainWindow = new MainWindow();
            Assert.Pass("Game launched!");
        }
    }
}