using NMines.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace NMines
{
    public static class Game
    {
        private static GameLevel level = GameLevel.HARD;
        private static GameConfig config;

        private static GameUI ui;
        private static Map map;
        private static MapWidget mapWidget;

        private static MainForm form;

        // этот флаг необходим в случае, если игрок выбирает уровень через комбобокс и отменяет выбор, чтобы не появлялось диалоговое окно с подтверждением
        // выбора уровня, который и так уже был выбран
        private static bool suppressSelectionChange = false;


        private static Dictionary<GameLevel, GameConfig> GetGameConfigs()
        {
            GameConfig easyGameConfig = new GameConfig(rowsCount: 9, colsCount: 9, minesCount: 10, cellSize: 60);
            GameConfig mediumGameConfig = new GameConfig(rowsCount: 16, colsCount: 16, minesCount: 40, cellSize: 43);
            GameConfig hardGameConfig = new GameConfig(rowsCount: 16, colsCount: 30, minesCount: 99, cellSize: 43);

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


        private static void AddGameLevelsToCombobox(GameUI ui)
        {
            foreach (GameLevel level in Enum.GetValues(typeof(GameLevel)))
            {
                ui.DifficultyCombobox.Items.Add(GameLevelToString(level));
            }
            ui.DifficultyCombobox.SelectedIndex = config.GetCurrentLevelIndex();
        }


        private static string GameLevelToString(GameLevel level)
        {
            string levelName = level.ToString().ToLower();
            return char.ToUpper(levelName[0]) + levelName.Substring(1);
        }

        private static void SetupGame()
        {
            var gameConfigs = GetGameConfigs();
            config = gameConfigs[level];
            ui = new GameUI(config);
            AddGameLevelsToCombobox(ui);
            ui.DifficultyCombobox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;
            SetGameLevelFromToolbar(ui);
        }

        private static void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressSelectionChange)
                return;

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

                map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
                map.InitField();

                if (mapWidget != null)
                {
                    GameUI.GameFieldPanel.Controls.Remove(mapWidget);
                    mapWidget.Dispose();
                }

                mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad);
                GameUI.GameFieldPanel.Controls.Add(mapWidget);
                mapWidget.ConfigureSize();

                GameUI.MinesCountLabel.Text = config.MinesCount.ToString();

                ui.RestartButton.Click += RestartButton_Click;

                GameUI.GameFieldPanel.Size = new Size(mapWidget.Width, mapWidget.Height);
                form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 40);
                form.Size = form.MinimumSize;
                form.MoveToCenter();

                GameUI.TimeLabel.ResetTimer();
            }
            else
            {
                suppressSelectionChange = true;
                comboBox.SelectedItem = GameLevelToString(level);
                suppressSelectionChange = false;
            }
        }


        private static void RestartButton_Click(object sender, EventArgs e)
        {
            map.InitField();
            mapWidget.Restart();
            mapWidget.Map.isFirstStep = true;
            GameUI.MinesCountLabel.Text = config.MinesCount.ToString();
            GameUI.TimeLabel.RestartTimer();
        }


        private static void CreateMainLayout(Form form)
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
            };

            GameUI.GameFieldPanel.Controls.Add(mapWidget);

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, GameUI.TopFieldHeight));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            mainLayout.Controls.Add(GameUI.GameToolbar, 0, 0);
            mainLayout.Controls.Add(GameUI.TopPanel, 0, 1);
            mainLayout.Controls.Add(GameUI.GameFieldPanel, 0, 2);

            form.Controls.Add(mainLayout);
        }


        private static void StartGame()
        {
            SetupGame();

            map = new Map(config.RowsCount, config.ColsCount, config.MinesCount);
            map.InitField();
            mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad);

            mapWidget.ConfigureSize();

            ui.RestartButton.Click += RestartButton_Click;

            form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 30);

            GameUI.GameFieldPanel.Size = new Size(mapWidget.Width, mapWidget.Height);

            CreateMainLayout(form);           
        }


        public static void Init(MainForm form)
        {
            Game.form = form;
            StartGame();
        }

        public static GameState GetGameState()
        {
            return new GameState
            {
                Level = level,
                Config = config,
                Map = map,
                GameTime = int.Parse(GameUI.TimeLabel.GetGameTime()),
            };
        }

        public static void SetGameState(GameState state)
        {
            if (state == null) return;

            bool levelChanged = state.Level != level;
            level = state.Level;
            config = state.Config;
            map = state.Map;

            if (levelChanged)
            {
                suppressSelectionChange = true;
                ui.DifficultyCombobox.SelectedIndex = config.GetCurrentLevelIndex();
                suppressSelectionChange = false;

                if (mapWidget != null)
                {
                    GameUI.GameFieldPanel.Controls.Remove(mapWidget);
                    mapWidget.Dispose();
                }

                mapWidget = new MapWidget(form, map, config.CellSize, config.XPad, config.YPad);
                GameUI.GameFieldPanel.Controls.Add(mapWidget);
                mapWidget.UpdateCells();

                ui.RestartButton.Click += RestartButton_Click;

                GameUI.GameFieldPanel.Size = new Size(mapWidget.Width, mapWidget.Height);
                form.MinimumSize = new Size(mapWidget.Width + 20, mapWidget.Height + 40);
                form.Size = form.MinimumSize;
            }

            GameUI.MinesCountLabel.Text = (config.MinesCount - map.CountFlaggedMines()).ToString();
            GameUI.TimeLabel.StopTimer();
            GameUI.TimeLabel.SetGameTime(state.GameTime);
            GameUI.TimeLabel.StartTimer();

            mapWidget.LoadSavedMap(map);
            mapWidget.ConfigureSize();
            form.MoveToCenter();
        }


    }
}
