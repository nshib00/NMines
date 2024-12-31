using System.Drawing;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class Cell : Button
    {
        private MapWidget mapWidget;
        public int value { get; set; }

        private bool isVisited;
        private bool isFlagged;

        public Cell(MapWidget mapWidget, int value, int size)
        {
            this.mapWidget = mapWidget;
            this.value = value;

            Dock = DockStyle.Fill;
            MinimumSize = new Size(size, size);
            Font = new Font("Segoe UI", 14);
            MouseDown += Cell_MouseDown;

            //Tag = new Point(i, j),
            //Location = new Point(j * cellSize + xPad, i * cellSize + topFieldHeight + yPad);
            // button.Size = new Size(cellSize, cellSize);
        }

        private void Cell_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    Button pressedButton = sender as Button;
                    pressedButton.Enabled = false;
                    SetTextAndColor();
                    if (value == -1)
                    {
                        MessageBox.Show("Вы проиграли!");
                        mapWidget.RevealCells();
                    }
                    else
                        BackColor = Color.White;
                    break;
                }
                case MouseButtons.Right:
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
                    break;
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

        public void SetToDefault()
        {
            BackColor = Color.WhiteSmoke;
            Text = "";
            Enabled = true;
        }
    }
}
