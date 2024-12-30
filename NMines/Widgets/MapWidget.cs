using NMines.Widgets;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public class MapWidget : TableLayoutPanel
    {
        private MainForm form;

        private Map map;
        private int cellSize;
        private int xPad;
        private int yPad;
        private int topFieldHeight;

        private Cell[,] cells;


        public MapWidget(MainForm form, Map map, int cellSize, int xPad, int yPad, int topFieldHeight)
        {
            this.form = form;
            this.map = map;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;
            this.topFieldHeight = topFieldHeight;

            RowCount = map.RowsCount;
            ColumnCount = map.ColsCount;

            Anchor = AnchorStyles.Top;

            cells = new Cell[map.RowsCount, map.ColsCount];

            AddStyles();
            Render();
        }

        public void InitCells()
        {
            for (int i = 0; i < map.RowsCount; i++)
            {
                for (int j = 0; j < map.ColsCount; j++)
                {
                    Cell cell = new Cell(size: cellSize, value: map.Field[i, j]);               

                    cell.SizeChanged += (sender, e) => MakeSquare(cell);

                    this.Controls.Add(cell, column: j, row: i);
                    cells[i, j] = cell;

                }
            }
        }


        public void UpdateCells()
        {
            for (int i = 0; i < map.RowsCount; i++)
            {
                for (int j = 0; j < map.ColsCount; j++)
                {
                    cells[i, j].Reset();

                    if (map.Field[i, j] == -1)
                    {
                        cells[i, j].Text = "*";
                        cells[i, j].BackColor = Color.Red;
                    }
                    else if (map.Field[i, j] != 0)
                        cells[i, j].Text = Convert.ToString(map.Field[i, j]);
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
            this.Width = map.ColsCount * cellSize + 5;
            this.Height = map.RowsCount * cellSize + topFieldHeight + 20;

            form.Width = this.Width + 30;
            form.Height = this.Height + 30;
        }

        private void AddStyles()
        {
            for (int i = 0; i < RowCount; i++)
                RowStyles.Add(new RowStyle(SizeType.Percent, 100f / map.RowsCount));

            for (int j = 0; j < ColumnCount; j++)
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / map.ColsCount));
        }


        private void Render()
        {
            form.Controls.Add(this);
        }

        public void Clear()
        {
            this.Controls.Clear();   
        }



    }
}
