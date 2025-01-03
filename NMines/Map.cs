using System;


namespace NMines
{
    public struct MapCell
    {
        public int Value;
        public bool IsOpened;

        public MapCell(int value = 0, bool isOpened = false)
        {
            Value = value;
            IsOpened = isOpened;
        }
    }

    public class Map
    {
        public int RowsCount { get; }
        public int ColsCount { get; }
        public int MinesCount { get; }
        public MapCell[,] Field { get; }

        public bool isFirstStep = true;

        public Map(int rowsCount, int colsCount, int minesCount)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;

            Field = new MapCell[RowsCount, ColsCount];
        }

        public void InitField()
        {
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

        private bool IsInBorder(int i, int j)
        {
            if (i < 0 || j < 0 || i > RowsCount - 1 || j > ColsCount - 1)
                return false;
            return true;
        }

    }
}
