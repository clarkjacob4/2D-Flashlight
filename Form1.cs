using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

//https://docs.microsoft.com/en-us/windows/win32/direct2d/how-to-clip-with-bitmap-masks
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        Panel oCanvas;
        bool bFoundPanel = false;
        Graphics oGraphics;
        Bitmap oBitmap;
        ImageAttributes imageAttr = new ImageAttributes();
        Rectangle rCanvas;
        Image myImages;
        Image myWallpaper;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oCanvas = Controls.Find("panel1", true).FirstOrDefault() as Panel;
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, oCanvas, new object[] { true });
            bFoundPanel = true;
            oBitmap = new Bitmap(oCanvas.Width, oCanvas.Height);
            myImages = Bitmap.FromFile(@"C:\Users\jclark\Working\VS\Images\Transparent.png");
            myWallpaper = Bitmap.FromFile(@"C:\Users\jclark\Working\VS\Images\Wallpaper.jpg");
            oGraphics = Graphics.FromImage(oBitmap);
            rCanvas = new Rectangle(0, 0, oCanvas.Width, oCanvas.Height);

            imageAttr.SetColorMatrix(new ColorMatrix(new float[][] { 
               new float[] {0, 0, 0, -1, 0},  // red scaling factor
               new float[] {0, 0, 0, 0, 0},  // green scaling factor
               new float[] {0, 0, 0, 0, 0},  // blue scaling factor
               new float[] {0, 0, 0, 1, 0},  // alpha scaling factor
               new float[] {-1, -1, -1, 0, 1}}),ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Point pCursor = oCanvas.PointToClient(Cursor.Position);
            oGraphics.Clear(Color.Black);
            oGraphics.DrawImage(myImages,50,50);
            oGraphics.DrawImage(myImages,pCursor.X - myImages.Width / 2,pCursor.Y - myImages.Height / 2);
            e.Graphics.DrawImage(myWallpaper,0,0,oCanvas.Width, oCanvas.Height);
            e.Graphics.DrawImage(oBitmap, rCanvas, 0, 0, oCanvas.Width, oCanvas.Height, GraphicsUnit.Pixel, imageAttr);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bFoundPanel == true) oCanvas.Invalidate();
        }
    }
}
