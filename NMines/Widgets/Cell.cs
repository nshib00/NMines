using System.Drawing;


namespace NMines.Widgets
{
    public class Cell
    {
        private MapWidget mapWidget;
        public int Value { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsFlagged { get; private set; }
        public bool IsHovered { get; set; }
        public bool IsExploded { get; set; }
        private int CellSize { get; set; }
        public Image Image { get; private set; }

        private readonly Font CellsFont = new Font("Segoe UI", 16);


        public Cell(MapWidget mapWidget, MapCell mapCell, int size, bool isHovered)
        {
            this.mapWidget = mapWidget;

            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
            IsHovered = isHovered;
            CellSize = size;
            Image = mapWidget.CellImages.ClosedCell;
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

        public void Close()
        {
            Image = mapWidget.CellImages.ClosedCell;
        }

        private void UpdateImage()
        {
            if (IsFlagged)
                Image = mapWidget.CellImages.Flag;
            else if (IsOpened)
            {
                Image = mapWidget.CellImages.EmptyCell;
                if (IsMine())
                {
                    if (IsExploded)
                        Image = mapWidget.CellImages.ExplodedMine;
                    else
                        Image = mapWidget.CellImages.Mine;
                }
                else if (Value > 0)
                    Image = mapWidget.CellImages.NumberCells[Value - 1];
            }
        }

        public void Draw(Graphics graphics, int x, int y)
        {
            UpdateImage();
            graphics.DrawImage(Image, x, y, CellSize, CellSize);
        }
    }
}
