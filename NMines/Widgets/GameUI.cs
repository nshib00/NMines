using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class GameUI
    {
        public Panel TopPanel { get; private set; }
        public FlowLayoutPanel GameFieldPanel { get; private set; }        
        public TimeLabel TimeLabel { get; private set; }
        public Label MinesCountLabel { get; private set; }
        public ToolStrip GameToolbar { get; private set; }
        public Button RestartButton { get; private set; }
        public ToolStripComboBox DifficultyCombobox { get; private set; }
        public int TopFieldHeight { get; private set; } = 45;

        private GameConfig config;


        public GameUI(GameConfig config)
        {
            this.config = config;

            InitTopPanel(config);
            InitGameFieldPanel();
        }


        private void InitGameToolbar()
        {
            GameToolbar = new ToolStrip()
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden
            };

            InitDifficultyCombobox();

            GameToolbar.Items.Add(new ToolStripLabel("Сложность"));
            GameToolbar.Items.Add(DifficultyCombobox);
            GameToolbar.Items.Add(new ToolStripSeparator());

            var saveButton = new ToolStripButton("Сохранить");
            saveButton.Click += GameToolbarSave_Click;
            saveButton.Font = new Font("Segoe UI", 8);
            GameToolbar.Items.Add(saveButton);

            GameToolbar.Items.Add(new ToolStripSeparator());

            var loadButton = new ToolStripButton("Загрузить");
            loadButton.Click += GameToolbarLoad_Click;
            loadButton.Font = new Font("Segoe UI", 8);
            GameToolbar.Items.Add(loadButton);

            GameToolbar.Items.Add(new ToolStripSeparator());

            var helpButton = new ToolStripButton("Справка");
            helpButton.Click += GameToolbarHelp_Click;
            helpButton.Font = new Font("Segoe UI", 8);
            GameToolbar.Items.Add(helpButton);
        }

        private void GameToolbarSave_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("data.bin", FileMode.Create))
            {
                GameState state = Game.GetGameState();
                formatter.Serialize(stream, state);
            }
            MessageBox.Show("Игра сохранена.");
        }

        private void GameToolbarLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("data.bin"))
            {
                MessageBox.Show("Нет сохраненной игры.");
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("data.bin", FileMode.Open))
            {
                GameState state = (GameState)formatter.Deserialize(stream);
                Game.SetGameState(state);
            }
            MessageBox.Show("Игра загружена.");
        }


        private void GameToolbarHelp_Click(object sender, EventArgs e)
        {
            TimeLabel.StopTimer();
            HelpForm helpForm = new HelpForm();
            helpForm.FormClosed += (s, args) => TimeLabel.StartTimer();
            helpForm.ShowDialog();
        }

        private void InitDifficultyCombobox()
        {
            DifficultyCombobox = new ToolStripComboBox();
            DifficultyCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void InitTopPanel(GameConfig config)
        {
            TopPanel = new Panel()
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

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));


            TimeLabel = new TimeLabel();

            RestartButton = new Button()
            {
                Text = "🙂",
                MinimumSize = new Size(40, 35),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15),
            };

            MinesCountLabel = new Label()
            {
                Text = config.MinesCount.ToString(),
                MinimumSize = new Size(40, 25),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18)
            };

            InitGameToolbar();

            layout.Controls.Add(TimeLabel, 0, 0);
            layout.Controls.Add(RestartButton, 1, 0);
            layout.Controls.Add(MinesCountLabel, 2, 0);

            TopPanel.Controls.Add(layout);
        }

        private void InitGameFieldPanel()
        {
            GameFieldPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.Top,
            };
        }
    }
}
