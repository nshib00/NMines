using System;
using System.Drawing;
using System.Windows.Forms;


namespace NMines.Widgets
{
    public class Cell
    {
        private MapWidget mapWidget;
        public int Value { get; set; }


        public bool isOpened { get; set; }
        public bool isFlagged { get; private set; }

        public Rectangle Rectangle { get; set; }

        public Cell(MapWidget mapWidget, int x, int y, int value, int size)
        {
            this.mapWidget = mapWidget;
            this.Value = value;

            this.Rectangle = new Rectangle(x, y, size, size);

            //Dock = DockStyle.Fill;
            //MinimumSize = new Size(size, size);
            //Font = new Font("Segoe UI", 14);
           // MouseUp += new MouseEventHandler(Cell_MouseUp);
        }

        private void Cell_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                {
                    OnLeftButtonClick();
                    break;
                }
                case MouseButtons.Right:
                {
                    OnRightButtonClick();
                    break;
                }
            }           
        }

        //private void Cell_Enter(object sender, EventArgs e)
        //{
        //    FlatAppearance.BorderSize = 5;
        //}

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    int currentCellX = GetRowIndex();
        //    int currentCellY = GetColumnIndex();

        //    switch (keyData)
        //    {
        //        case Keys.Up:
        //            {
        //                int newRow = currentCellX;
        //                do
        //                {
        //                    newRow = (newRow - 1 + mapWidget.Map.RowsCount) % mapWidget.Map.RowsCount;
        //                } while (!mapWidget.GetCell(newRow, currentCellY).Enabled);

        //                mapWidget.SelectCell(newRow, currentCellY);
        //                return true;
        //            }
        //        case Keys.Down:
        //            {
        //                int newRow = currentCellX;
        //                do
        //                {
        //                    newRow = (newRow + 1) % mapWidget.Map.RowsCount;
        //                } while (!mapWidget.GetCell(newRow, currentCellY).Enabled);

        //                mapWidget.SelectCell(newRow, currentCellY);
        //                return true;
        //            }
        //        case Keys.Left:
        //            {
        //                int newCol = currentCellY;
        //                do
        //                {
        //                    newCol = (newCol - 1 + mapWidget.Map.ColsCount) % mapWidget.Map.ColsCount;
        //                } while (!mapWidget.GetCell(currentCellX, newCol).Enabled);

        //                mapWidget.SelectCell(currentCellX, newCol);
        //                return true;
        //            }
        //        case Keys.Right:
        //            {
        //                int newCol = currentCellY;
        //                do
        //                {
        //                    newCol = (newCol + 1) % mapWidget.Map.ColsCount;
        //                } while (!mapWidget.GetCell(currentCellX, newCol).Enabled);

        //                mapWidget.SelectCell(currentCellX, newCol);
        //                return true;
        //            }
        //        case Keys.W:
        //        case Keys.Enter:
        //        {
        //            OnLeftButtonClick();
        //            return true;
        //        }
        //        case Keys.E:
        //        {
        //            OnRightButtonClick();
        //            return true;
        //        }

        //        default:
        //            return base.ProcessCmdKey(ref msg, keyData);
        //    }
        //}

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);

        //    if (!isFlagged)
        //        this.BackColor = Color.LightBlue;
        //}

        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //    if (!isFlagged)
        //        this.BackColor = SystemColors.Control;
        //}


        //public void SimulateMouseEnter()
        //{
        //    OnMouseEnter(EventArgs.Empty);

        //    FlatAppearance.BorderSize = 2;

        //    FlatAppearance.BorderColor = Color.Blue;
        //}

        //public void FocusCell()
        //{
        //    Focus();
        //}


        private void OnLeftButtonClick()
        {
            if (mapWidget.Map.isFirstStep)
            {
                //int firstMoveX = ((Point)Tag).X;
                //int firstMoveY = ((Point)Tag).Y;

                //mapWidget.Map.SeedMines(firstMoveX, firstMoveY);
                //mapWidget.Map.CountMinesAroundCells();
               // mapWidget.UpdateCells();
                mapWidget.Map.isFirstStep = false;
            }

            //this.SetTextAndColor();
            //this.Enabled = false;

            if (Value == -1)
            {
                //mapWidget.RevealCells();
                MessageBox.Show("You lose.");
            }
            else
            {
                //BackColor = Color.White;
                //mapWidget.OpenCellWithEmptyNeighbors(this);
            }
        }

        private void OnRightButtonClick()
        {
            //if (!isFlagged)
            //{
            //    PutFlag();
            //    mapWidget.DecreaseMinesCount();
            //    isFlagged = true;
            //}
            //else
            //{
            //    RemoveFlag();
            //    mapWidget.IncreaseMinesCount();
            //    isFlagged = false;
            //    SetClosed();
            //}
            //if (mapWidget.CountFlaggedMines() == mapWidget.Map.MinesCount)
            //{
            //    mapWidget.RevealCells();
            //    MessageBox.Show("You win!");
            //}
        }


        //public void SetTextAndColor()
        //{
        //    if (value == -1)
        //    {
        //        //Text = "*";
        //        BackColor = Color.Brown;
        //    }
        //    else if (value != 0)
        //    {
        //        Text = value.ToString();
        //        BackColor = Color.WhiteSmoke;
        //    } 
        //}


        //private void SetClosed()
        //{
        //    Text = "";
        //    BackColor = Color.WhiteSmoke;
        //}


        public void Check()
        {
            // меняем image на empty, если в клетке 0, иначе на значение в клетке
        }

        //public void PutFlag()
        //{
        //    isFlagged = true;
        //    Text = "F";
        //    BackColor = Color.Crimson;
            //minesCountLabel.Text = ((int)(minesCountLabel.Text) - 1).ToString();
            // меняем image на флаг
        //}

        //public void RemoveFlag()
        //{
        //    SetTextAndColor();
        //    if (value == 0)
        //        Text = "";
        //    BackColor = Color.WhiteSmoke;
        //    isFlagged = false;
        //    // меняем image с флага на значение в клетке
        //}

        //public void SetToDefault()
        //{
        //    BackColor = Color.WhiteSmoke;
        //    Text = "";
        //    Enabled = true;
        //}

        //public int GetRowIndex()
        //{
        //    return ((Point)Tag).X;
        //}

        //public int GetColumnIndex()
        //{
        //    return ((Point)Tag).Y;
        //}
    }
}
