using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public class Game
    {
        private GameLevel level;
        private GameConfig config;

        private Map map;
        private MapWidget mapWidget;

        public Game(GameLevel level)
        {
            this.level = level;
        }

        private Dictionary<GameLevel, GameConfig> GetGameConfigs()
        {
            GameConfig easyGameConfig = new GameConfig(rowsCount: 9, colsCount: 9, minesCount: 10, cellSize: 60, topFieldHeight: 50);
            GameConfig mediumGameConfig = new GameConfig(rowsCount: 16, colsCount: 16, minesCount: 40, cellSize: 45, topFieldHeight: 50);
            GameConfig hardGameConfig = new GameConfig(rowsCount: 16, colsCount: 30, minesCount: 99, cellSize: 45, topFieldHeight: 50);

            return new Dictionary<GameLevel, GameConfig>()
            {
                { GameLevel.EASY, easyGameConfig },
                { GameLevel.MEDIUM, mediumGameConfig },
                { GameLevel.HARD, hardGameConfig },
            };
        }

        private void SetupGame()
        {
            var gameConfigs = GetGameConfigs();
            config = gameConfigs[level];
        }

        private Panel CreateTopPanel()
        {
            Panel topPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray,
            };

            TableLayoutPanel layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
            };

            // layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            //ComboBox changeLevelBox = new ComboBox()
            //{
            //    DataSource = GetGameConfigs().Values.ToList(),
            //};

            Label timeLabel = new Label()
            {
                Text = "00:00",
                MinimumSize = new Size(40, 30),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18)
            };

            Button restartButton = new Button()
            {
                Text = "🙂",
                MinimumSize = new Size(40, 40),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18),
            };
            restartButton.Click += restartButton_Click;

            Label minesCountLabel = new Label()
            {
                Text = config.MinesCount.ToString(),
                MinimumSize = new Size(40, 30),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18)
            };


            //layout.Controls.Add(changeLevelBox, 0, 0);
            layout.Controls.Add(timeLabel, 0, 0);
            layout.Controls.Add(restartButton, 1, 0);
            layout.Controls.Add(minesCountLabel, 2, 0);

            topPanel.Controls.Add(layout);

            return topPanel;
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            map.InitField();
            mapWidget.UpdateCells();
            mapWidget.Map.isFirstStep = true;
        }

        public void Init(MainForm form)
        {
            SetupGame();


            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, config.TopFieldHeight));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Panel topPanel = CreateTopPanel();
            mainLayout.Controls.Add(topPanel, 0, 0);

            map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
            mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad, config.TopFieldHeight);
            map.InitField();
            //map.SeedMines();
            //map.CountMinesAroundCells();

            mapWidget.ConfigureSize();
            mapWidget.InitCells();

            form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 20);

            mainLayout.Controls.Add(mapWidget, 0, 1);

            form.Controls.Add(mainLayout);
        }
    }
}
