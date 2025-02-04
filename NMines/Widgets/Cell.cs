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
        public Image Image { get; set; }

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


        public void RemoveFlag()
        {
            Image = mapWidget.CellImages.ClosedCell;
        }


        public void Draw(Graphics graphics, int x, int y)
        {
            Color cellColor = DefaultColor;

            if (IsHovered)
            {
                cellColor = HoveredColor;
            }

            if (IsFlagged)
            {
                Image = mapWidget.CellImages.Flag;
            }
            else if (IsOpened)
            {
                Image = mapWidget.CellImages.EmptyCell;
                if (IsMine())
                {
                    if (IsExploded)
                    {
                        Image = mapWidget.CellImages.ExplodedMine;
                    }
                    else
                    {
                        Image = mapWidget.CellImages.Mine;
                    }
                }
                else if (Value > 0)
                    Image = mapWidget.CellImages.NumberCells[Value - 1];
            }

            graphics.DrawImage(Image, x, y, CellSize, CellSize);
        }
    }
}
