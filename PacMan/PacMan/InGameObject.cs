using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PacMan
{
    public abstract class InGameObject
    {
        protected String Tag { get; set; }
        protected double Width { get; set; }
        protected double Height { get; set; }
        protected double Top { get; set; }
        protected double Left { get; set; }

        public InGameObject()
        {
            this.Tag = "Default";
            this.Width = 0;
            this.Height = 0;
            this.Top = 0;
            this.Left = 0;
        }

        public abstract void addToCanvas(Canvas canvas);

    }

    public class Wall : InGameObject
    {
        
        private double StrokeThickness { get; set; }
        private Brush Stroke { get; set; }

        public Wall(string tag, double width, double height, double top, double left, double strokeThickness, string stroke)
        {
            this.Tag = tag;
            this.Width = width;
            this.Height = height;
            var converter = new BrushConverter();
            this.StrokeThickness = strokeThickness;
            this.Stroke = (Brush)converter.ConvertFromString(stroke);

            this.Top = top;
            this.Left = left;
            
        }

        public override void addToCanvas(Canvas canvas)
        {
            Rectangle rect = new Rectangle();
            rect.Tag = this.Tag;
            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.Stroke = this.Stroke;
            rect.StrokeThickness = this.StrokeThickness;


            canvas.Children.Add(rect);
            Canvas.SetTop(rect, this.Top);
            Canvas.SetLeft(rect, this.Left);

        }
    }

    public class Coin : InGameObject
    {
        private Brush Fill { get; set; }

        public Coin(string tag, double width, double height, double top, double left, string fill)
        {
            this.Tag = tag;
            this.Width = width;
            this.Height = height;
            var converter = new BrushConverter();
            this.Fill = (Brush)converter.ConvertFromString(fill);

            this.Top = top;
            this.Left = left;

        }

        public override void addToCanvas(Canvas canvas)
        {
            Rectangle rect = new Rectangle();
            rect.Tag = this.Tag;
            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.Fill = this.Fill;


            canvas.Children.Add(rect);
            Canvas.SetTop(rect, this.Top);
            Canvas.SetLeft(rect, this.Left);

        }
    }
}
