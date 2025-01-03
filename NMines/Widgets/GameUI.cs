using System.Drawing;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class GameUI
    {
        public Panel TopPanel { get; private set; }
        public static Label TimeLabel { get; private set; }
        public static Label MinesCountLabel { get; private set; }
        public Button RestartButton { get; private set; }
       
        public static int TopFieldHeight { get; private set; }

        public GameUI(GameConfig config)
        {
            InitTopPanel(config);
            TopFieldHeight = 50;
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

            // layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            //ComboBox changeLevelBox = new ComboBox()
            //{
            //    DataSource = GetGameConfigs().Values.ToList(),
            //};

            TimeLabel = new Label()
            {
                Text = "00:00",
                MinimumSize = new Size(40, 30),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18)
            };

            RestartButton = new Button()
            {
                Text = "🙂",
                MinimumSize = new Size(40, 40),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18),
            };

            MinesCountLabel = new Label()
            {
                Text = config.MinesCount.ToString(),
                MinimumSize = new Size(40, 30),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18)
            };


            //layout.Controls.Add(changeLevelBox, 0, 0);
            layout.Controls.Add(TimeLabel, 0, 0);
            layout.Controls.Add(RestartButton, 1, 0);
            layout.Controls.Add(MinesCountLabel, 2, 0);

            TopPanel.Controls.Add(layout);
        }
    }
}
