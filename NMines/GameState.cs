using NMines.Widgets;
using System;


namespace NMines
{
    [Serializable]
    public class GameState
    {
        public GameLevel Level { get; set; }
        public GameConfig Config { get; set; }
        public Map Map { get; set; }
        public int GameTime { get; set; }
    }
}
