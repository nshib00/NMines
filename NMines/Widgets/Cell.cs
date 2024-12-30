using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NMines.Widgets
{
    public class Cell : Button
    {
        private int value;
        private bool isVisited;
        private bool isFlagged;

        public Cell(int value, int size)
        {
            this.value = value;

            Dock = DockStyle.Fill;
            MinimumSize = new Size(size, size);
            Font = new Font("Segoe UI", 14);
            Click += Cell_Click;
            MouseDown += Cell_MouseDown;

            SetTextAndColor();    
            //Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
            //Margin = new Padding(5, 0, 5, 0),
            //Tag = new Point(i, j),
            //Location = new Point(j * cellSize + xPad, i * cellSize + topFieldHeight + yPad);
            // button.Size = new Size(cellSize, cellSize);
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (value == -1)
                MessageBox.Show("Вы проиграли!");
        }

        private void Cell_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!isFlagged)
                {
                    PutFlag();
                    isFlagged = true;
                }
                else
                {
                    RemoveFlag();
                    isFlagged = false;
                }
            }            
        }

        private void SetTextAndColor()
        {
            if (value == -1)
            {
                Text = "*";
                BackColor = Color.Red;
            }
            else if (value != 0)
            {
                Text = value.ToString();
                BackColor = Color.WhiteSmoke;
            } 
        }


        public void Check()
        {
            isVisited = true;
            // меняем image на empty, если в клетке 0, иначе на значение в клетке
        }

        public void PutFlag()
        {
            isFlagged = true;
            Text = "F";
            BackColor = Color.Crimson;
            //minesCountLabel.Text = ((int)(minesCountLabel.Text) - 1).ToString();
            // меняем image на флаг
        }

        public void RemoveFlag()
        {
            SetTextAndColor();
            if (value == 0)
                Text = "";
            BackColor = Color.WhiteSmoke;
            isFlagged = false;
            // меняем image с флага на значение в клетке
        }

        public void Reset()
        {
            value = 0;
            BackColor = Color.WhiteSmoke;
            Text = "";
        }
    }
}
