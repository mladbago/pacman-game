using System;

namespace PacMan
{
    internal interface IGameMethods
    {
        void GameOver(string message);
        void GameLoop(object sender, EventArgs e);
        void GameSetUp();
    }
}
