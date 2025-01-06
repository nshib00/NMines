using NMines.Widgets;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NMines
{
    public class MapWidget : TableLayoutPanel
    {
        private MainForm form;

        public Map Map { get; private set; }
        private int cellSize;
        private int xPad;
        private int yPad;

        private int topFieldHeight;

        private Cell[,] cells;

        public Image tileset;


        public MapWidget(MainForm form, Map map, int cellSize, int xPad, int yPad)
        {
            this.form = form;
            this.Map = map;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;

            this.topFieldHeight = 50;

            //string tilesetPath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Images/tiles.png");
            //string tilesetPath = @"D:/projects/csharp/NMines/NMines/Images/tiles.png";
            //tileset = new Bitmap(tilesetPath);

            RowCount = Map.RowsCount;
            ColumnCount = Map.ColsCount;

            Anchor = AnchorStyles.Top;

            cells = new Cell[Map.RowsCount, Map.ColsCount];

            //DoubleBuffered = true;

            AddStyles();
            Render();
        }





        public void InitCells()
        {
            SuspendLayout();
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    Cell cell = new Cell(this, size: cellSize, value: Map.Field[i, j].Value);
                    cell.Tag = new Point(i, j);

                    cell.SizeChanged += (sender, e) => MakeSquare(cell);

                    this.Controls.Add(cell, column: j, row: i);
                    cells[i, j] = cell;

                }
            }
            cells[Map.RowsCount / 2, Map.ColsCount / 2].FocusCell(); // в начале игры фокус на ячейке в середине поля
            ResumeLayout();
        }


        public void UpdateCells()
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    cells[i, j].SetToDefault();
                    cells[i, j].value = Map.Field[i, j].Value;
                }
            }
        }

        public void ClearCells()
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    cells[i, j] = default;
                }
            }
            Controls.Clear();
        }


        private void MakeSquare(Control control)
        {
            int size = Math.Min(control.Width, control.Height);
            control.Width = size;
            control.Height = size;
        }

        public void ConfigureSize()
        {
            this.Width = Map.ColsCount * cellSize + 5;
            this.Height = Map.RowsCount * cellSize + topFieldHeight + 20;

            form.Width = this.Width + 30;
            form.Height = this.Height + 30;
        }

        private void AddStyles()
        {
            for (int i = 0; i < RowCount; i++)
                RowStyles.Add(new RowStyle(SizeType.Percent, 100f / Map.RowsCount));

            for (int j = 0; j < ColumnCount; j++)
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / Map.ColsCount));
        }

        public void OpenCell(int rowIndex, int columnIndex)
        {
            if (Map.Field[rowIndex, columnIndex].Value == -1)
            {
                cells[rowIndex, columnIndex].Text = "*";
                cells[rowIndex, columnIndex].BackColor = Color.Red;
            }
            else if (Map.Field[rowIndex, columnIndex].Value != 0)
                cells[rowIndex, columnIndex].Text = Convert.ToString(Map.Field[rowIndex, columnIndex].Value);

            Map.Field[rowIndex, columnIndex].IsOpened = true;
            cells[rowIndex, columnIndex].Enabled = false;
        }

        public void OpenCell(Cell cell)
        {
            int rowIndex = cell.GetRowIndex();
            int columnIndex = cell.GetColumnIndex();
            OpenCell(rowIndex, columnIndex);
        }

        public void OpenCellWithEmptyNeighbors(Cell cell)
        {
            if (cell.value > 0)
                return;

            cell.SetTextAndColor();
            cell.Enabled = false;

            int row = cell.GetRowIndex();
            int col = cell.GetColumnIndex();

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (!Map.IsInBorder(i, j))
                        continue;

                    Cell neighbor = cells[i, j];

                    if (neighbor.isFlagged || !neighbor.Enabled)
                        continue;

                    if (neighbor.value == 0)
                        OpenCellWithEmptyNeighbors(neighbor);
                    OpenCell(neighbor);
                }
            }

        }


        public Cell GetCell(int rowIndex, int columnIndex)
        {
            return cells[rowIndex, columnIndex];
        }


        public void SelectCell(int rowIndex, int columnIndex)
        {
            cells[rowIndex, columnIndex].FocusCell();
        }

        public void RevealCells()
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    OpenCell(i, j);
                }
            }
        }


        private void Render()
        {
            form.Controls.Add(this);
        }

        public void Clear()
        {
            this.Controls.Clear();   
        }

        public void IncreaseMinesCount()
        {
            GameUI.MinesCountLabel.Text = (GetMinesCount() + 1).ToString();
        }

        public void DecreaseMinesCount()
        {
            GameUI.MinesCountLabel.Text = (GetMinesCount() - 1).ToString();
        }

        public int GetMinesCount()
        {
            return int.Parse(GameUI.MinesCountLabel.Text);
        }


        public int CountFlaggedMines()
        {
            int flaggedMines = 0;

            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    if (cells[i, j].isFlagged && cells[i, j].value == -1)
                        flaggedMines++;
                }
            }
            return flaggedMines;
        }

        public Image GetCellImage(int x, int y)
        {
            Bitmap image = new Bitmap(cellSize, cellSize);
            Graphics graphics = Graphics.FromImage(image);
           // graphics.DrawImage();

            return image;
        }


    }
}
