using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacMan
{
    public partial class MainWindow : Window, IGameMethods
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); 

        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp; 

        int speed = 8;

        Rect pacmanHitBox; 

        int ghostSpeed = 10; 
        int ghostMoveStep = 160; 
        int currentGhostStep; 
        int score = 0; 



        public MainWindow()
        {
            InitializeComponent();
            GameSetUp(); 
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left && noLeft == false)
            {

                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false; 

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); 

            }

            if (e.Key == Key.Up && noUp == false)
            {
 
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false; 

                goUp = true; 

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false;
                goUp = goLeft = goRight = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }

        public void GameSetUp()
        {
            initMap(MyCanvas);
            MyCanvas.Focus(); 

            gameTimer.Tick += GameLoop; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start(); 
            currentGhostStep = ghostMoveStep;

            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;


        }

        public void initMap(Canvas canvas)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\map_1_walls.txt");
            string[] lines = File.ReadAllLines(@path);
            foreach(string line in lines)
            {
                string[] data = line.Split(';');
                Wall wall = new Wall(data[0], Convert.ToDouble(data[4]), Convert.ToDouble(data[3]), Convert.ToDouble(data[6]), Convert.ToDouble(data[5]), Convert.ToDouble(data[2]), data[1]);
                wall.addToCanvas(MyCanvas);

            }

            path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\map_1_coins.txt");
            lines = File.ReadAllLines(@path);
            foreach (string line in lines)
            {
                string[] data = line.Split(';');
                Coin coin = new Coin(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]), Convert.ToDouble(data[5]), Convert.ToDouble(data[4]), data[3]);
                coin.addToCanvas(MyCanvas);

            }
        }

        public void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score; 

            if (goRight){
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft){
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp){
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown){
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }

            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height){
                noDown = true;
                goDown = false;
            }

            if (goUp && Canvas.GetTop(pacman) < 1){
                noUp = true;
                goUp = false;
            }

            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1){
                noLeft = true;
                goLeft = false;
            }

            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width){
                noRight = true;
                goRight = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); 

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }

                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }

                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }

                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }


                if ((string)x.Tag == "coin")
                {

                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {

                        x.Visibility = Visibility.Hidden;

                        score++;
                    }
                }

                if ((string)x.Tag == "ghost")
                {

                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("Enemies have killed you :( If you want to try again - press 'OK'");
                    }


                    if (x.Name.ToString() == "orangeGuy")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }

            if (score == 85)
            {
                GameOver("Congrats! Collected all coins!");
            }
        }

        public void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "PacMan Game");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
