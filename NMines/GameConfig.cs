using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMines
{
    public class GameConfig
    {
        public int RowsCount { get; }
        public int ColsCount { get; }
        public int MinesCount { get; }
        public int CellSize { get; }
        public int XPad { get; }
        public int YPad { get; }

        public int TopFieldHeight { get; }

        public GameConfig(int rowsCount, int colsCount, int minesCount, int cellSize, int topFieldHeight, int xPad = 5, int yPad = 5)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;
            CellSize = cellSize;
            XPad = xPad;
            YPad = yPad;
            TopFieldHeight = topFieldHeight;
        }
    }
}
