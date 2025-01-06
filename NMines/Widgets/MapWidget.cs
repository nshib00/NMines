using NMines.Widgets;
using System.Drawing;
using System.Windows.Forms;


namespace NMines
{
    public class MapWidget : Control
    {
        private MainForm form;
        public Map Map { get; private set; }

        private int cellSize;
        private int xPad;
        private int yPad;
        private int topFieldHeight;

        private readonly Color DefaultColor = Color.LightGray;
        private readonly Color HoveredColor = Color.LightBlue;
        private readonly Color MineColor = Color.Red;
        private readonly Color OpenedColor = Color.White;
        private readonly Color FlaggedColor = Color.Brown;

        private readonly Font CellsFont = new Font("Segoe UI", 16);

        private int hoveredRow = -1;
        private int hoveredCol = -1;

        private bool isGameOver = false;

        public MapWidget(MainForm form, Map map, int cellSize, int xPad, int yPad)
        {
            this.form = form;
            this.Map = map;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;
            this.topFieldHeight = 50;

            MouseMove += MapWidget_MouseMove;
            MouseClick += MapWidget_MouseClick;
            DoubleBuffered = true;

            ConfigureSize();
        }

        private void MapWidget_MouseClick(object sender, MouseEventArgs e)
        {
            int row = (e.Y - yPad) / cellSize;
            int col = (e.X - xPad) / cellSize;

            if (row >= 0 && row < Map.RowsCount && col >= 0 && col < Map.ColsCount)
            {
                if (e.Button == MouseButtons.Left)
                {
                    OnLeftButtonClick(row, col);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    OnRightButtonClick(row, col);
                }
            }
        }

        private void OnLeftButtonClick(int row, int col)
        {
            if (Map.isFirstStep)
            {
                Map.SeedMines(row, col);
                Map.CountMinesAroundCells();
                Map.isFirstStep = false;
            }

            if (!isGameOver && !Map.Field[row, col].IsFlagged)
            {
                OpenCell(row, col);
                Invalidate();

                if (Map.Field[row, col].Value == -1 && !Map.Field[row, col].IsFlagged)
                {
                    isGameOver = true;
                    RevealCells();
                    MessageBox.Show("You lose.");
                }
            }
        }

        private void OnRightButtonClick(int row, int col)
        {
            if (!Map.Field[row, col].IsFlagged)
            {
                Map.Field[row, col].IsFlagged = true;
                DecreaseMinesCount();
            }
            else
            {
                Map.Field[row, col].IsFlagged = false;
                IncreaseMinesCount();
            }

            Invalidate();

            if (Map.CountFlaggedMines() == Map.MinesCount)
            {
                RevealCells();
                MessageBox.Show("You win!");
            }
        }

        private void MapWidget_MouseMove(object sender, MouseEventArgs e)
        {
            int row = (e.Y - yPad) / cellSize;
            int col = (e.X - xPad) / cellSize;

            if (row != hoveredRow || col != hoveredCol)
            {
                hoveredRow = row;
                hoveredCol = col;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMap(e.Graphics);
        }

        private void DrawMap(Graphics g)
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    int x = xPad + j * cellSize;
                    int y = yPad + i * cellSize;

                    Color cellColor = DefaultColor;
                    if (i == hoveredRow && j == hoveredCol)
                    {
                        cellColor = HoveredColor;
                    }

                    if (Map.Field[i, j].IsFlagged)
                    {
                        cellColor = FlaggedColor;
                    }

                    if (Map.Field[i, j].IsOpened)
                    {
                        cellColor = OpenedColor;
                        if (Map.Field[i, j].Value == -1)
                        {
                            cellColor = MineColor;
                        }
                    }

                    using (Brush brush = new SolidBrush(cellColor))
                    {
                        g.FillRectangle(brush, x, y, cellSize, cellSize);
                    }

                    using (Pen pen = new Pen(Color.Black))
                    {
                        g.DrawRectangle(pen, x, y, cellSize, cellSize);
                    }

                    if (Map.Field[i, j].IsOpened)
                    {
                        string text = "";
                        if (Map.Field[i, j].Value == -1)
                            text = "*";
                        else if (Map.Field[i, j].Value > 0)
                            text = Map.Field[i, j].Value.ToString();
                        TextRenderer.DrawText(g, text, CellsFont, new Point(x + cellSize / 5, y + cellSize / 5), Color.Black);
                    }
                }
            }

            if (isGameOver)
            {
                using (Brush overlayBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                {
                    g.FillRectangle(overlayBrush, xPad, yPad, Map.ColsCount * cellSize, Map.RowsCount * cellSize);
                }
            }
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


        private void OpenCell(int row, int col)
        {
            if (Map.Field[row, col].IsOpened) return;
            Map.Field[row, col].IsOpened = true;

            if (Map.Field[row, col].Value == 0)
            {
                OpenEmptyNeighbors(row, col);
            }
        }

        private void OpenEmptyNeighbors(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < Map.RowsCount && j >= 0 && j < Map.ColsCount && !Map.Field[i, j].IsOpened && Map.Field[i, j].Value != -1)
                    {
                        OpenCell(i, j);
                    }
                }
            }
        }

        public void ConfigureSize()
        {
            Width = Map.ColsCount * cellSize + xPad * 2 + 10;
            Height = Map.RowsCount * cellSize + yPad * 2 + topFieldHeight + 25;

            form.Width = Width + 20;
            form.Height = Height + 40;
        }

        public void RestartWidget()
        {
            isGameOver = false;
            hoveredRow = -1;
            hoveredCol = -1;
            Invalidate();
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
