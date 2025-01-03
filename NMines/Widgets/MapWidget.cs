using NMines.Widgets;
using System;
using System.Drawing;
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


        public MapWidget(MainForm form, Map map, int cellSize, int xPad, int yPad, int topFieldHeight)
        {
            this.form = form;
            this.Map = map;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;
            this.topFieldHeight = topFieldHeight;

            RowCount = Map.RowsCount;
            ColumnCount = Map.ColsCount;

            Anchor = AnchorStyles.Top;

            cells = new Cell[Map.RowsCount, Map.ColsCount];

            AddStyles();
            Render();
        }

        public void InitCells()
        {
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
        }


        public void UpdateCells()
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    cells[i, j].SetToDefault();
                    cells[i, j].value = Map.Field[i, j].Value;
                    cells[i, j].Text = cells[i, j].value.ToString();
                }
            }
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

        public void OpenCell(Cell cell)
        {
            int rowIndex = ((Point)cell.Tag).X;
            int columnIndex = ((Point)cell.Tag).Y;
            OpenCell(rowIndex, columnIndex);
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


    }
}
