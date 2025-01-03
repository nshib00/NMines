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
            MouseUp += new MouseEventHandler(Cell_MouseUp);

            Text = value.ToString();
            //Location = new Point(j * cellSize + xPad, i * cellSize + topFieldHeight + yPad);
            // button.Size = new Size(cellSize, cellSize);
        }

        private void Cell_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    if (mapWidget.Map.isFirstStep)
                    {
                        int firstMoveX = ((Point)Tag).X;
                        int firstMoveY = ((Point)Tag).Y;

                        mapWidget.Map.SeedMines(firstMoveX, firstMoveY);
                        mapWidget.Map.CountMinesAroundCells();
                        mapWidget.UpdateCells();
                        mapWidget.Map.isFirstStep = false;
                    }
                    this.SetTextAndColor();
                    this.Enabled = false;
                    
                    if (value == -1)
                    {
                        mapWidget.RevealCells();
                        MessageBox.Show("Вы проиграли!");
                    }
                    else
                        BackColor = Color.White;
                    mapWidget.OpenCell(this);
                    break;
                }
                case MouseButtons.Right:
                {
                    if (!isFlagged)
                    {
                        PutFlag();
                        mapWidget.DecreaseMinesCount();
                        isFlagged = true;
                    }
                    else
                    {
                        RemoveFlag();
                        mapWidget.IncreaseMinesCount();
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
                BackColor = Color.Brown;
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
