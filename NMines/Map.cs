using System;


namespace NMines
{
    [Serializable]
    public struct MapCell
    {
        public int Value;
        public bool IsOpened;
        public bool IsFlagged;

        public MapCell(int value = 0, bool isOpened = false, bool isFlagged = false)
        {
            Value = value;
            IsOpened = isOpened;
            IsFlagged = isFlagged;
        }
    }

    [Serializable]
    public class Map
    {
        public int RowsCount { get; }
        public int ColsCount { get; }
        public int MinesCount { get; }
        public MapCell[,] Field { get; private set; }

        public bool isFirstStep = true;

        public Map(int rowsCount, int colsCount, int minesCount)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;
        }

        public void InitField()
        {
            Field = new MapCell[RowsCount, ColsCount];

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    Field[i, j] = new MapCell();
                }
            }
        }

        public void SeedMines(int firstMoveX, int firstMoveY)
        {
            Random rand = new Random();

            for (int i = 0; i < MinesCount; i++)
            {
                int randX = rand.Next(0, RowsCount);
                int randY = rand.Next(0, ColsCount);

                while (Field[randX, randY].Value == -1 || (Math.Abs(randX - firstMoveX) <= 1 && Math.Abs(randY - firstMoveY) <= 1))
                {
                    randX = rand.Next(0, RowsCount);
                    randY = rand.Next(0, ColsCount);
                }

                Field[randX, randY].Value = -1;
            }

        }


        public void CountMinesAroundCells()
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    if (Field[i, j].Value == -1)
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int l = j - 1; l <= j + 1; l++)
                            {
                                if (!IsInBorder(k, l) || Field[k, l].Value == -1)
                                    continue;
                                Field[k, l].Value++;
                            }
                        }
                    }
                }
            }
        }

        public bool IsInBorder(int i, int j)
        {
            if (i < 0 || j < 0 || i > RowsCount - 1 || j > ColsCount - 1)
                return false;
            return true;
        }

        public int CountFlaggedMines()
        {
            int flaggedMines = 0;

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    if (Field[i, j].IsFlagged && Field[i, j].Value == -1)
                        flaggedMines++;
                }
            }

            return flaggedMines;
        }

        public void OpenCell(int row, int col)
        {
            if (Field[row, col].IsOpened) return;
            Field[row, col].IsOpened = true;

            if (Field[row, col].Value == 0)
            {
                OpenEmptyNeighbors(row, col);
            }
        }

        public void OpenEmptyNeighbors(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < RowsCount && j >= 0 && j < ColsCount && !Field[i, j].IsOpened && Field[i, j].Value != -1)
                    {
                        OpenCell(i, j);
                    }
                }
            }
        }

    }
}
