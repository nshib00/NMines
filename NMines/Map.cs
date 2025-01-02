using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMines
{
    public class Map
    {
        public int RowsCount { get; }
        public int ColsCount { get; }
        private int MinesCount { get; }
        public int[,] Field { get; }

        public bool isFirstStep = true;

        public Map(int rowsCount, int colsCount, int minesCount)
        {
            RowsCount = rowsCount;
            ColsCount = colsCount;
            MinesCount = minesCount;

            Field = new int[RowsCount, ColsCount];
        }

        public void InitField()
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    Field[i, j] = 0;
                }
            }
        }

        public void SeedMines()
        {
            Random rand = new Random();

            for (int i = 0; i < MinesCount; i++)
            {
                int randX = rand.Next(0, RowsCount - 1);
                int randY = rand.Next(0, ColsCount - 1);

                while (Field[randX, randY] == -1)
                {
                    randX = rand.Next(0, RowsCount - 1);
                    randY = rand.Next(0, ColsCount - 1);
                }

                Field[randX, randY] = -1;
            }

        }


        public void CountMinesAroundCells()
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    if (Field[i, j] == -1)
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int l = j - 1; l <= j + 1; l++)
                            {
                                if (!IsInBorder(k, l) || Field[k, l] == -1)
                                    continue;
                                Field[k, l]++;
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
