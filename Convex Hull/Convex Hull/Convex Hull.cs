using System.Runtime.InteropServices;

namespace Convex_Hull
{
    

    public partial class Form1 : Form
    {
        static List<List<Point>> list = new List<List<Point>>();
        static int CurrentStep = 0, TotalSteps = 0;
        Label Total = new Label(), Current = new Label(),NumberOfPoints=    new Label();
        private static void ConvexHull(List<Point> Points)
        {
            if (Points.Count == 0) return;

            Points=Points.OrderBy(Point => Point.X).ToList();
            List<Point> Hull = new List<Point>();

            foreach (var pt in Points)
            {
                while (Hull.Count >= 2 && !Ccw(Hull[Hull.Count - 2], Hull[Hull.Count - 1], pt))
                {
                    Hull.RemoveAt(Hull.Count - 1);
                }
                Hull.Add(pt);
                printHull(Hull);
                list.Add(Hull.ToList());
                TotalSteps++;
            }
            int t = Hull.Count + 1;
            for (int i = Points.Count - 1; i >= 0; i--)
            {
                Point pt = Points[i];
                while (Hull.Count >= t && !Ccw(Hull[Hull.Count - 2], Hull[Hull.Count - 1], pt))
                {
                    Hull.RemoveAt(Hull.Count - 1);
                }
                Hull.Add(pt);
                printHull(Hull);
                list.Add(Hull.ToList());
                TotalSteps++;
            }



        }
        static int counter = 1;
        public static void printHull(List<Point> Hull) {
            Console.WriteLine($"Hull {counter++}");
            foreach (Point point in Hull) {
                Console.WriteLine(point);
            }
            Console.WriteLine("------------------------------------------");
        }
        private static bool Ccw(Point A, Point B, Point C)
        {
            return ((B.X -A.X) * (C.Y - A.Y)) > ((B.Y - A.Y) * (C.X - A.X));
        }

        int width = 30;
        static List<Point> points = new List<Point>();
        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            AutoSize = true;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.MouseDown += Form1_MouseDown;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AllocConsole();
            //To Stop Flickering.
            this.SetStyle(
               System.Windows.Forms.ControlStyles.UserPaint |
               System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
               System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
               true);

            //Setting up labels.
            Current.Location = new Point(800, 950);
            Current.Font = new Font(Current.Font.FontFamily, 30);
            Current.Size = new Size(350, 50);

            Total.Location= new Point(1200, 950);
            Total.Font = new Font(Total.Font.FontFamily, 30);
            Total.Size = new Size(350, 50);

            NumberOfPoints.Location = new Point(300, 950);
            NumberOfPoints.Font = new Font(NumberOfPoints.Font.FontFamily, 30);
            NumberOfPoints.Size = new Size(500, 50);

            this.Controls.Add(Total);
            this.Controls.Add(Current);
            this.Controls.Add(NumberOfPoints);

            Setup();

            

        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void Setup() {
            TotalSteps = 0;
            CurrentStep = 0;
            select = 0;
            list = new List<List<Point>>();
            int n = rand.Next(31)+10;   //Max 40.
            points.Clear();
            //Generating Random Points
            for (int i = 0; i < n; i++)
            {
                points.Add(new Point(rand.Next(1501)+200, rand.Next(801)+100));
            }

            //Using LinQ to sort points by X value.
            points = points.OrderBy(Point => Point.X).ToList();
            ConvexHull(points);
            TotalSteps--;
            NumberOfPoints.Text = "Number of points: "+n.ToString();
            Total.Text = "Total Steps: " + TotalSteps.ToString();

        }

        int select = 0;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                if (select < list.Count-1)
                {
                    select++;
                    CurrentStep++;
                    Current.Text = CurrentStep.ToString();
                }
                else
                {
                    Setup();
                }
            Invalidate();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (select < list.Count - 1)
                {
                    select++;
                    CurrentStep++;
                    Current.Text = CurrentStep.ToString();
                }
                else
                {
                    Setup();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (select > 0)
                {
                    select--;
                    CurrentStep--;
                    Current.Text = CurrentStep.ToString();
                }
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            Pen pen = new Pen(Color.Blue, 10);
            Brush brush = new SolidBrush(Color.Black);
            List<Point> linepoints = list.ElementAt(select);
            int CircleSize = 15;

            //Draw Lines.
            for(int i=linepoints.Count-1; i>=1; i--) {
                e.Graphics.DrawLine(pen, linepoints[i].X+(CircleSize/2), linepoints[i].Y+(CircleSize/2), linepoints[i - 1].X + (CircleSize / 2), linepoints[i-1].Y + (CircleSize / 2));
            }

            //Draw Points.
            foreach (Point point in points)
            {
                e.Graphics.FillEllipse(brush, point.X, point.Y, CircleSize, CircleSize);
            }
            //Update Step.
            Current.Text= "Current Step: "+CurrentStep.ToString();
        }
    }
}