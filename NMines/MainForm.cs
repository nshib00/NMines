using System;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public partial class MainForm : Form
    {
        // сделать отдельный класс контроллер поля

        private const GameLevel gameLevel = GameLevel.HARD;
        public Game game = new Game(gameLevel);

        public MainForm()
        {
            InitializeComponent();
            Font = new Font("Segoe UI", 20); 
            StartPosition = FormStartPosition.CenterScreen;

            game.Init(this);
        }
    }
}
