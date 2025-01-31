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

        public GameConfig(int rowsCount, int colsCount, int minesCount, int cellSize, int xPad = 5, int yPad = 5)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;
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
}
