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

        private static MainForm form;


        private static Dictionary<GameLevel, GameConfig> GetGameConfigs()
        {
            GameConfig easyGameConfig = new GameConfig(rowsCount: 9, colsCount: 9, minesCount: 10, cellSize: 60);
            GameConfig mediumGameConfig = new GameConfig(rowsCount: 16, colsCount: 16, minesCount: 40, cellSize: 45);
            GameConfig hardGameConfig = new GameConfig(rowsCount: 16, colsCount: 30, minesCount: 99, cellSize: 45);

            return new Dictionary<GameLevel, GameConfig>()
            {
                { GameLevel.EASY, easyGameConfig },
                { GameLevel.MEDIUM, mediumGameConfig },
                { GameLevel.HARD, hardGameConfig },
            };
        }


        private static void SetGameLevelFromToolbar(GameUI gameUI)
        {
            switch (gameUI.DifficultyCombobox.SelectedIndex)
            {
                case 0:
                    level = GameLevel.EASY;
                    break;
                case 1:
                    level = GameLevel.MEDIUM;
                    break;
                case 2:
                    level = GameLevel.HARD;
                    break;
            }
        }


        private static void SetupGame()
        {
            var gameConfigs = GetGameConfigs();
            config = gameConfigs[level];
            ui = new GameUI(config);
            ui.DifficultyCombobox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;
            SetGameLevelFromToolbar(ui);
        }

        private static void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;
            string selectedDifficulty = comboBox.SelectedItem.ToString();

            string caption = $"You select difficulty \"{selectedDifficulty}\".";
            string messageToShow = "The game will be restarted. Are you sure?";
            var confirmResult = MessageBox.Show(messageToShow, caption, MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                SetGameLevelFromToolbar(ui);
                var gameConfigs = GetGameConfigs();
                config = gameConfigs[level];

                MessageBox.Show(level.ToString());

                mapWidget.ClearCells();

                map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
                map.InitField();

                mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad);
                mapWidget.ConfigureSize();
                mapWidget.InitCells();

                GameUI.MinesCountLabel.Text = config.MinesCount.ToString();

                ui.RestartButton.Click += RestartButton_Click;

                form.MoveToCenter();
            }
        }

        private static void RestartButton_Click(object sender, EventArgs e)
        {
            map.InitField();
            mapWidget.UpdateCells();
            mapWidget.Map.isFirstStep = true;
            GameUI.MinesCountLabel.Text = config.MinesCount.ToString();
        }


        private static void CreateMainLayout(Form form)
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, GameUI.TopFieldHeight));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainLayout.Controls.Add(GameUI.GameToolbar, 0, 0);
            mainLayout.Controls.Add(GameUI.TopPanel, 0, 1);
            mainLayout.Controls.Add(mapWidget, 0, 2);

            form.Controls.Add(mainLayout);
        }


        private static void StartGame()
        {
            SetupGame();

            map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
            mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad);
            map.InitField();

            mapWidget.ConfigureSize();
            mapWidget.InitCells();

            ui.RestartButton.Click += RestartButton_Click;

            form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 30);

            CreateMainLayout(form);
        }


        public static void Init(MainForm form)
        {
            Game.form = form;
            StartGame();
        }
    }
}
