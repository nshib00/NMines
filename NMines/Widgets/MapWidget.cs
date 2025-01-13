using NMines.Widgets;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace NMines
{
    public class MapWidget : Panel
    {
        private MainForm form;
        public Map Map { get; private set; }

        private int cellSize;
        private int xPad;
        private int yPad;
        private int topFieldHeight;

        private int hoveredRow = -1;
        private int hoveredCol = -1;

        private int keyboardHoveredRow = -1;
        private int keyboardHoveredCol = -1;

        private bool isGameOver = false;

        public MapWidget(MainForm form, Map map, int cellSize, int xPad, int yPad)
        {
            this.form = form;
            this.Map = map;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;

            topFieldHeight = 50;
            keyboardHoveredRow = Map.RowsCount / 2;
            keyboardHoveredCol = Map.ColsCount / 2;

            MouseMove += MapWidget_MouseMove;
            MouseClick += MapWidget_MouseClick;

            DoubleBuffered = true;

            ConfigureSize();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Focus();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int currentCellX = keyboardHoveredRow;
            int currentCellY = keyboardHoveredCol;

            switch (keyData)
            {
                case Keys.Up:
                    MoveSelection(-1, 0);
                    return true;
                case Keys.Down:
                    MoveSelection(1, 0);
                    return true;
                case Keys.Left:
                    MoveSelection(0, -1);
                    return true;
                case Keys.Right:
                    MoveSelection(0, 1);
                    return true;
                case Keys.W:
                    PerformLeftClick();
                    return true;
                case Keys.E:
                    PerformRightClick();
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void PerformLeftClick()
        {
            if (isGameOver) return;

            OnLeftButtonClick(keyboardHoveredRow, keyboardHoveredCol);
        }

        private void PerformRightClick()
        {
            if (isGameOver) return;

            OnRightButtonClick(keyboardHoveredRow, keyboardHoveredCol);
        }

        private void MoveSelection(int dRow, int dCol)
        {
            if (isGameOver) return;

            int newRow = hoveredRow;
            int newCol = hoveredCol;

            do
            {
                newRow = (newRow + dRow + Map.RowsCount) % Map.RowsCount;
                newCol = (newCol + dCol + Map.ColsCount) % Map.ColsCount;
            }
            while (Map.Field[newRow, newCol].IsOpened && !(newRow == hoveredRow && newCol == hoveredCol));

            keyboardHoveredRow = newRow;
            keyboardHoveredCol = newCol;

            // синхронизация координат мыши с координатами клавиатуры
            hoveredRow = newRow;
            hoveredCol = newCol;

            Invalidate();
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
                GameUI.TimeLabel.StartTimer();
            }

            if (!isGameOver && !Map.Field[row, col].IsFlagged)
            {
                OpenCell(row, col);
                Invalidate();

                if (Map.Field[row, col].Value == -1)
                {
                    isGameOver = true;
                    GameUI.TimeLabel.StopTimer();
                    RevealCells();
                    MessageBox.Show("You lose.");
                }
            }
        }

        private void OnRightButtonClick(int row, int col)
        {
            if (!Map.Field[row, col].IsOpened)
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
                    GameUI.TimeLabel.StopTimer();
                    RevealCells();

                    string gameTime = GameUI.TimeLabel.GetGameTime();
                    MessageBox.Show($"You win!\nGame time: {gameTime} seconds."); 
                }
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

                // синхронизация координат клавиатуры с координатами мыши
                keyboardHoveredRow = row;
                keyboardHoveredCol = col;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMap(e.Graphics);
        }

        private void DrawMap(Graphics graphics)
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    int x = xPad + j * cellSize;
                    int y = yPad + i * cellSize;

                    bool isMouseHovered = (i == hoveredRow && j == hoveredCol);
                    bool isKeyboardHovered = (i == keyboardHoveredRow && j == keyboardHoveredCol);

                    Cell cell = new Cell(this, mapCell: Map.Field[i, j], size: cellSize, isHovered: isMouseHovered);
                    cell.Draw(graphics, x, y);

                    if (isKeyboardHovered)
                    {
                        using (Pen pen = new Pen(Color.Green, 2))
                        {
                            graphics.DrawRectangle(pen, x, y, cellSize, cellSize);
                        }
                    }
                }
            }

            if (isGameOver)
            {
                using (Brush overlayBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                {
                    graphics.FillRectangle(overlayBrush, xPad, yPad, Map.ColsCount * cellSize, Map.RowsCount * cellSize);
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

        public void Restart()
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
