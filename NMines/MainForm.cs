using System;
using System.Drawing;
using System.Windows.Forms;

namespace NMines
{
    public partial class MainForm : Form
    {
        // сделать отдельный класс контроллер поля

        public MainForm()
        {
            KeyPreview = true;

            InitializeComponent();
            Font = new Font("Segoe UI", 20); 
            StartPosition = FormStartPosition.CenterScreen;
            
            Game.Init(this);
        }
    }
}
