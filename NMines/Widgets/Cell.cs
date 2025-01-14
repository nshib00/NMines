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
        public Image Image { get; set; }

        private readonly Color DefaultColor = Color.LightGray;
        private readonly Color HoveredColor = Color.LightBlue;
        private readonly Color MineColor = Color.Red;
        private readonly Color OpenedColor = Color.White;
        private readonly Color FlaggedColor = Color.Brown;

        private readonly Font CellsFont = new Font("Segoe UI", 16);


        public Cell(MapWidget mapWidget, MapCell mapCell, int size, bool isHovered, Image image)
        {
            this.mapWidget = mapWidget;

            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
            IsHovered = isHovered;
            CellSize = size;
            Image = image;
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

        public void RemoveFlag()
        {
            Image = mapWidget.FindCellImage(1, 3);
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
                Image = mapWidget.FindCellImage(1, 2);
            }

            if (IsOpened)
            {
                Image = mapWidget.FindCellImage(1, 4);
                if (IsMine())
                {
                    Image = mapWidget.FindCellImage(1, 0);
                }
            }

            if (IsOpened)
            {
                if (IsMine())
                    Image = mapWidget.FindCellImage(1, 0);
                else if (Value > 0)
                    Image = mapWidget.FindCellImage(0, Value - 1);
            }

            graphics.DrawImage(Image, x, y, CellSize, CellSize);
        }
    }
}
