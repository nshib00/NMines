using System;

namespace NMines
{
    [Serializable]
    public class GameConfig
    {
        public int RowsCount { get; }
        public int ColsCount { get; }
        public int MinesCount { get; }
        public int CellSize { get; }
        public int XPad { get; }
        public int YPad { get; }

        public GameConfig(int rows, int cols, int mines, int cellSize, int xPad = 5, int yPad = 5)
        {
            RowsCount = rows;
            ColsCount = cols;
            MinesCount = mines;
            CellSize = cellSize;
            XPad = xPad;
            YPad = yPad;
        }

        public int GetCurrentLevelIndex()
        {
            switch (MinesCount)
            {
                case 10:
                    return 0;
                case 40:
                    return 1;
                case 99:
                    return 2;
                default:
                    return 0;
            }
        }
    }

    [Serializable]
    public class EasyGameConfig : GameConfig
    {
        public EasyGameConfig() : base(rows: 9, cols: 9, mines: 10, cellSize: 60) { }
    }

    [Serializable]
    public class MediumGameConfig : GameConfig
    {
        public MediumGameConfig() : base(rows: 16, cols: 16, mines: 40, cellSize: 43) { }
    }

    [Serializable]
    public class HardGameConfig : GameConfig
    {
        public HardGameConfig() : base(rows: 16, cols: 30, mines: 99, cellSize: 43) { }
    }
}
