using NMines.Widgets;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace NMines
{
    public class MapWidget : Panel
    {
        private MainForm form;
        private Cell[,] cells;

        private const int CellImageSize = 64;

        public Map Map { get; private set; }

        private GameUI ui;
        public ImageCache CellImages { get; private set; }

        private int cellSize;
        private int xPad;
        private int yPad;
        private int topFieldHeight;

        private int hoveredRow = -1;
        private int hoveredCol = -1;

        private int keyboardHoveredRow;
        private int keyboardHoveredCol;

        private bool blockMouseHover = false;

        private bool isGameOver = false;

        public delegate void CellEventHandler(int row, int col);
        public delegate void EndGameHandler(bool isVictory);

        public event CellEventHandler LeftClick;
        public event CellEventHandler RightClick;
        public event CellEventHandler CellHovered;
        public event EndGameHandler GameOver;

        public MapWidget(MainForm form, Map map, GameUI ui, int cellSize, int xPad, int yPad)
        {
            this.form = form;
            this.Map = map;
            this.ui = ui;
            this.cellSize = cellSize;
            this.xPad = xPad;
            this.yPad = yPad;

            topFieldHeight = 50;
            keyboardHoveredRow = Map.RowsCount / 2;
            keyboardHoveredCol = Map.ColsCount / 2;

            MouseMove += MapWidget_MouseMove;
            MouseClick += MapWidget_MouseClick;
            KeyDown += MapWidget_KeyDown;

            LeftClick += OnLeftButtonClick;
            RightClick += OnRightButtonClick;
            GameOver += OnGameOver;
            CellHovered += OnCellHover;

            DoubleBuffered = true;

            ConfigureSize();

            CellImages = new ImageCache(cellSize, CellImageSize);
            InitCells();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Focus();
        }

        private void MapWidget_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;

            switch (e.KeyCode)
            {
                case Keys.W:
                    MoveSelection(-1, 0);
                    break;
                case Keys.S:
                    MoveSelection(1, 0);
                    break;
                case Keys.A:
                    MoveSelection(0, -1);
                    break;
                case Keys.D:
                    MoveSelection(0, 1);
                    break;
                case Keys.O:
                    LeftClick?.Invoke(keyboardHoveredRow, keyboardHoveredCol);
                    blockMouseHover = true;
                    break;
                case Keys.P:
                    RightClick?.Invoke(keyboardHoveredRow, keyboardHoveredCol);
                    blockMouseHover = true;
                    break;
                case Keys.I:
                    blockMouseHover = !blockMouseHover;
                    break;
            }
        }

        private void InitCells()
        {
            cells = new Cell[Map.RowsCount, Map.ColsCount];
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    int x = xPad + j * cellSize;
                    int y = yPad + i * cellSize;

                    cells[i, j] = new Cell(this, mapCell: Map.Field[i, j], size: cellSize, isHovered: false);
                }
            }
        }


        public void UpdateCells()
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    cells[i, j].UpdateState(Map.Field[i, j]);
                }
            }
            Invalidate();
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
            while (Map.Field[newRow, newCol].IsOpened && !(newRow == keyboardHoveredRow && newCol == keyboardHoveredCol));

            keyboardHoveredRow = hoveredRow = newRow;
            keyboardHoveredCol = hoveredCol = newCol;

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
                    LeftClick?.Invoke(row, col);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    RightClick?.Invoke(row, col);
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
                ui.TimeLabel.StartTimer();
            }

            if (!isGameOver && !Map.Field[row, col].IsFlagged)
            {
                Map.OpenCell(row, col);
                UpdateCells();

                if (Map.Field[row, col].Value == -1)
                {
                    isGameOver = true;
                    cells[row, col].IsExploded = true;
                    GameOver?.Invoke(isVictory: false);                    
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
                    cells[row, col].Close();
                    IncreaseMinesCount();
                }

                UpdateCells();

                if (Map.CountFlaggedMines() == Map.MinesCount)
                {
                    GameOver?.Invoke(isVictory: true);
                }
            }
        }

        private void OnGameOver(bool isVictory)
        {
            ui.TimeLabel.StopTimer();
            RevealCells();
            string msgText;

            if (isVictory)
            {
                string gameTime = ui.TimeLabel.GetGameTime();
                msgText = $"Вы выиграли!\nВремя: {gameTime} с.";
            }
            else
                msgText = "Вы проиграли.";

            MessageBox.Show(msgText);
        }


        private void OnCellHover(int row, int col)
        {
            hoveredRow = row;
            hoveredCol = col;

            // синхронизация координат клавиатуры с координатами мыши
            keyboardHoveredRow = row;
            keyboardHoveredCol = col;

            Invalidate();
        }


        private void MapWidget_MouseMove(object sender, MouseEventArgs e)
        {
            if (!blockMouseHover)
            {
                int row = (e.Y - yPad) / cellSize;
                int col = (e.X - xPad) / cellSize;

                if (row != hoveredRow || col != hoveredCol)
                {
                    CellHovered?.Invoke(row, col);
                }
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

                    cells[i, j].IsHovered = isKeyboardHovered;
                    cells[i, j].Draw(graphics, x, y);

                    if (isKeyboardHovered || (isMouseHovered && blockMouseHover))
                    {
                        using (Pen pen = new Pen(Color.Black, 2))
                        {
                            graphics.DrawRectangle(pen, x - 1, y - 1, cellSize, cellSize);
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
                    Map.OpenCell(i, j);
                }
            }
            UpdateCells();
        }


        public void CloseCells()
        {
            for (int i = 0; i < Map.RowsCount; i++)
            {
                for (int j = 0; j < Map.ColsCount; j++)
                {
                    cells[i, j].Close();
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
            CloseCells();
            UpdateCells();
        }

        public void IncreaseMinesCount()
        {
            ui.MinesCountLabel.Text = (GetMinesCount() + 1).ToString();
        }

        public void DecreaseMinesCount()
        {
            ui.MinesCountLabel.Text = (GetMinesCount() - 1).ToString();
        }

        public int GetMinesCount()
        {
            return int.Parse(ui.MinesCountLabel.Text);
        }

        public void LoadSavedMap(Map map)
        {
            Map = map;
            isGameOver = false;

            InitCells();
            UpdateCells();
            Invalidate();
        }
    }
}
