using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class GameUI
    {
        public static Panel TopPanel { get; private set; }
        public static FlowLayoutPanel GameFieldPanel { get; private set; }        
        public static TimeLabel TimeLabel { get; private set; }
        public static Label MinesCountLabel { get; private set; }
        public static ToolStrip GameToolbar { get; private set; }

        public Button RestartButton { get; private set; }
        public ToolStripComboBox DifficultyCombobox { get; private set; }

        public static int TopFieldHeight { get; private set; } = 45;


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

            GameToolbar.Items.Add(new ToolStripLabel("Difficulty"));
            GameToolbar.Items.Add(DifficultyCombobox);
            GameToolbar.Items.Add(new ToolStripSeparator());

            var saveButton = new ToolStripButton("Save game");
            saveButton.Click += GameToolbarSave_Click;
            saveButton.Font = new Font("Segoe UI", 8);
            GameToolbar.Items.Add(saveButton);

            GameToolbar.Items.Add(new ToolStripSeparator());

            var loadButton = new ToolStripButton("Load game");
            loadButton.Click += GameToolbarLoad_Click;
            loadButton.Font = new Font("Segoe UI", 8);
            GameToolbar.Items.Add(loadButton);
        }

        private void GameToolbarSave_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("data.bin", FileMode.Create))
            {
                GameState state = Game.GetGameState();
                formatter.Serialize(stream, state);
            }
            MessageBox.Show("The game is saved.");
        }

        private void GameToolbarLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("data.bin"))
            {
                MessageBox.Show("No saved game found.");
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("data.bin", FileMode.Open))
            {
                GameState state = (GameState)formatter.Deserialize(stream);
                Game.SetGameState(state);
            }
            MessageBox.Show("The game is loaded.");
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
