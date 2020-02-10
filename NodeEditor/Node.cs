using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media.Effects;

namespace WpfApp1 {
    public class Node {
        public Size size;
        public Color color;
        public Point position;

        public event Action<Node> OnDelete = delegate { };
        public event Action<Node, MouseEventArgs> OnMoved = delegate { };
        public event Action<Node, MouseButtonEventArgs> OnClicked = delegate { };

        private Rectangle rect = new Rectangle();

        public Node(Size size, Color color, Point position) {
            this.size = size;
            this.color = color;
            this.position = position;

            BindEvents();
            AddContextMenu();
        }

        public void Add(ref Canvas canvas) {
            canvas.Children.Add(GetRect());
        }

        public void Remove(ref Canvas canvas) {
            canvas.Children.Remove(GetRect());
        }

        public Rectangle GetRect() {
            return rect;
        }

        private void BindEvents() {
            rect.MouseMove += (object sender, MouseEventArgs e) => { OnMoved(this, e); };
            rect.MouseDown += (object sender, MouseButtonEventArgs e) => { OnClicked(this, e); };
        }

        private void AddContextMenu() {
            MenuItem mi = new MenuItem();
            mi.Header = "Delete";
            mi.Name = "Mi";
            mi.Click += Mi_Click;
            mi.DataContext = rect;
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add(mi);
            rect.ContextMenu = contextMenu;
        }

        public void Render() {
            DropShadowEffect effect = new DropShadowEffect();
            effect.BlurRadius = 10;
            effect.ShadowDepth = 2;
            rect.Effect = effect;
            rect.Stroke = new SolidColorBrush(color);
            rect.Fill = new SolidColorBrush(color);
            rect.Width = size.Width;
            rect.Height = size.Height;
            SetPosition(position);
        }

        public Point GetPosition() {
            return position;
        }

        public void SetPosition(Point position) {
            Canvas.SetLeft(rect, position.X - rect.Width * 0.5f);
            Canvas.SetTop(rect, position.Y - rect.Height * 0.5f);
            this.position = position;
        }

        public void SetZIndex(int index) {
            Canvas.SetZIndex(rect, index);
        }

        public int GetZIndex() {
            return Canvas.GetZIndex(rect);
        }

        private void Mi_Click(object sender, RoutedEventArgs e) {
            MenuItem mi = sender as MenuItem;
            if (mi == null) return;
            Rectangle rect = mi.DataContext as Rectangle;
            if (rect == null) return;
            OnDelete(this);
        }
    }
}
