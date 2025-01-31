using System.Drawing;
using System.IO;


namespace NMines.Widgets
{
    public class ImageCache
    {
        private Image cellImageSet;

        public Image Mine { get; private set; }
        public Image Flag { get; private set; }
        public Image ClosedCell { get; private set; }
        public Image EmptyCell { get; private set; }
        public Image[] NumberCells { get; private set; }

        private int CellSize;
        private int CellImageSize;


        public ImageCache(int cellSize, int cellImageSize)
        {
            CellSize = cellSize;
            CellImageSize = cellImageSize;
            NumberCells = new Image[8];

            string imagePath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Images/tiles.jpg");
            cellImageSet = new Bitmap(imagePath);

            for (int i = 0; i < 8; i++)
            {
                NumberCells[i] = FindCellImage(0, i);
            }
            Mine = FindCellImage(1, 0);
            Flag = FindCellImage(1, 2);
            ClosedCell = FindCellImage(1, 3);
            EmptyCell = FindCellImage(1, 4);
        }

        public Image FindCellImage(int xPos, int yPos)
        {
            Image image = new Bitmap(CellSize, CellSize);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                Rectangle sourceRect = new Rectangle(CellImageSize * yPos, CellImageSize * xPos, CellImageSize, CellImageSize);
                Rectangle destRect = new Rectangle(0, 0, CellSize, CellSize);
                graphics.DrawImage(cellImageSet, destRect, sourceRect, GraphicsUnit.Pixel);
            }

            return image;
        }
    }
}
