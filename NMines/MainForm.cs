using System;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            KeyPreview = true;

            InitializeComponent();
            Font = new Font("Segoe UI", 20); 
            StartPosition = FormStartPosition.CenterScreen;
            
            Game.Init(this);
        }

        public void MoveToCenter()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            int x = (screenWidth - Width) / 2;
            int y = (screenHeight - Height) / 2;
            
            Location = new Point(x, y);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
