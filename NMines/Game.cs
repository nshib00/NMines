using NMines.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public static class Game
    {
        private static GameLevel level = GameLevel.EASY;
        private static GameConfig config;

        private static GameUI ui;
        private static Map map;
        private static MapWidget mapWidget;


        private static Dictionary<GameLevel, GameConfig> GetGameConfigs()
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

        private static void SetupGame()
        {
            var gameConfigs = GetGameConfigs();
            config = gameConfigs[level];
            ui = new GameUI(config);
        }

        private static void RestartButton_Click(object sender, EventArgs e)
        {
            map.InitField();
            mapWidget.UpdateCells();
            mapWidget.Map.isFirstStep = true;
            GameUI.MinesCountLabel.Text = config.MinesCount.ToString();
        }

        public static void Init(MainForm form)
        {
            SetupGame();

            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
            };

            map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
            mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad, config.TopFieldHeight);
            map.InitField();

            mapWidget.ConfigureSize();
            mapWidget.InitCells();

            ui.RestartButton.Click += RestartButton_Click;

            form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 20);

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, config.TopFieldHeight));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainLayout.Controls.Add(ui.TopPanel, 0, 0);
            mainLayout.Controls.Add(mapWidget, 0, 1);

            form.Controls.Add(mainLayout);
        }
    }
}
