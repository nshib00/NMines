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

        public GameConfig(int rowsCount, int colsCount, int minesCount, int cellSize, int xPad = 5, int yPad = 5)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;
            CellSize = cellSize;
            XPad = xPad;
            YPad = yPad;
        }
    }
}
