using System;
using System.Drawing;
using System.Timers;
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
            GameToolbar.Items.Add(new ToolStripLabel("Save game"));
            GameToolbar.Items.Add(new ToolStripSeparator());
            GameToolbar.Items.Add(new ToolStripLabel("Load game"));
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
