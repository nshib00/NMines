using System;
using System.Drawing;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class CellClickEventArgs : EventArgs
    {
        public int Value { get; }
        public bool IsOpened { get; }
        public bool IsFlagged { get; }

        public CellClickEventArgs(int value, bool isOpened, bool isFlagged)
        {
            Value = value;
            IsOpened = isOpened;
            IsFlagged = isFlagged;
        }
    }

    public class CellHoverEventArgs : CellClickEventArgs
    {
        public CellHoverEventArgs(int value, bool isOpened, bool isFlagged) : base(value, isOpened, isFlagged) {}
    }

    public class Cell : Control
    {
        private MapWidget mapWidget;
        public int Value { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsFlagged { get; private set; }
        public bool IsHovered { get; set; }
        private int Size { get; set; }

        private readonly Color DefaultColor = Color.LightGray;
        private readonly Color HoveredColor = Color.LightBlue;
        private readonly Color MineColor = Color.Red;
        private readonly Color OpenedColor = Color.White;
        private readonly Color FlaggedColor = Color.Brown;

        private readonly Font CellsFont = new Font("Segoe UI", 16);

        public event EventHandler<CellClickEventArgs> LeftClicked;
        public event EventHandler<CellClickEventArgs> RightClicked;
        public event EventHandler<CellHoverEventArgs> Hovered;


        public Cell(MapWidget mapWidget, MapCell mapCell, int size, bool isHovered)
        {
            this.mapWidget = mapWidget;

            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
            IsHovered = isHovered;
            Size = size;

            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            //Tag = mapCell.GetTag();
        }


        private void Open()
        {
            IsOpened = true;
            Invalidate();
        }

        private void ToggleFlag()
        {
            IsFlagged = !IsFlagged;
            Invalidate();
        }

        public void UpdateState(MapCell mapCell)
        {
            Value = mapCell.Value;
            IsFlagged = mapCell.IsFlagged;
            IsOpened = mapCell.IsOpened;
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.W && !IsOpened)
            {
                Open();
                LeftClicked?.Invoke(this, new CellClickEventArgs(Value, IsOpened, IsFlagged));
            }
            else if (e.KeyCode == Keys.E && !IsOpened)
            {
                ToggleFlag();
                RightClicked?.Invoke(this, new CellClickEventArgs(Value, IsOpened, IsFlagged));
            }
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
                graphics.FillRectangle(brush, x, y, Size, Size);
            }

            using (Pen pen = new Pen(Color.Black, borderWidth))
            {
                graphics.DrawRectangle(pen, x, y, Size, Size);
            }

            if (IsOpened)
            {
                string text = "";
                if (IsMine())
                    text = "*";
                else if (Value > 0)
                    text = Value.ToString();
                TextRenderer.DrawText(graphics, text, CellsFont, new Point(x + Size / 5, y + Size / 5), Color.Black);
            }

            Invalidate();
        }
    }
}
