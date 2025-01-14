using System;
using System.Drawing;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class Cell
    {
        private MapWidget mapWidget;
        public int Value { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsFlagged { get; private set; }
        public bool IsHovered { get; set; }
        private int CellSize { get; set; }

        private readonly Color DefaultColor = Color.LightGray;
        private readonly Color HoveredColor = Color.LightBlue;
        private readonly Color MineColor = Color.Red;
        private readonly Color OpenedColor = Color.White;
        private readonly Color FlaggedColor = Color.Brown;

        private readonly Font CellsFont = new Font("Segoe UI", 16);


        public Cell(MapWidget mapWidget, MapCell mapCell, int size, bool isHovered)
        {
            this.mapWidget = mapWidget;

            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
            IsHovered = isHovered;
            CellSize = size;

            //Tag = mapCell.GetTag();
        }

        public void UpdateState(MapCell mapCell)
        {
            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
        }

        public bool IsMine()
        {
            return (Value == -1);
        }

        public void Draw(Graphics graphics, int x, int y)
        {
            Color cellColor = DefaultColor;
            byte borderWidth = 1;

            if (IsHovered)
            {
                cellColor = HoveredColor;
                borderWidth = 2;
            }
            if (IsFlagged)
            {
                cellColor = FlaggedColor;
            }

            if (IsOpened)
            {
                cellColor = OpenedColor;
                if (IsMine())
                {
                    cellColor = MineColor;
                }
            }

            using (Brush brush = new SolidBrush(cellColor))
            {
                graphics.FillRectangle(brush, x, y, CellSize, CellSize);
            }

            using (Pen pen = new Pen(Color.Black, borderWidth))
            {
                graphics.DrawRectangle(pen, x, y, CellSize, CellSize);
            }

            if (IsOpened)
            {
                string text = "";
                if (IsMine())
                    text = "*";
                else if (Value > 0)
                    text = Value.ToString();
                TextRenderer.DrawText(graphics, text, CellsFont, new Point(x + CellSize / 5, y + CellSize / 5), Color.Black);
            }
        }
    }
}
