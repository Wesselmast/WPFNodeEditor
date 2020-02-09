using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media.Effects;

namespace WpfApp1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<Node> nodes = new List<Node>();

        public MainWindow() {
            InitializeComponent();
        }

        private void spawnNode_Click(object sender, RoutedEventArgs e) {
            DrawNode(new Size(100, 50), (Color)ColorConverter.ConvertFromString("#FFf39204"), Mouse.GetPosition(canvas));
        }

        private void DrawNode(Size size, Color color, Point position) {
            Node node = new Node(size, color, position);

            BindNode(node);
            nodes.Add(node);
            node.Add(ref canvas);

            node.Render();
        }

        private void Node_OnClicked(Node node, MouseButtonEventArgs e) {
            foreach(Node n in nodes) {
                if (n == node) continue;
                n.SetZIndex(n.GetZIndex() - 1);
            }
            node.SetZIndex(nodes.Count);
        }

        private void Node_OnMoved(Node node, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                node.SetPosition(e.GetPosition(canvas));
            }
        }

        private void Node_OnDelete(Node node) {
            UnbindNode(node);
            nodes.Remove(node);
            node.Remove(ref canvas);
        }

        private void BindNode(Node node) {
            node.OnMoved += Node_OnMoved;
            node.OnClicked += Node_OnClicked;
            node.OnDelete += Node_OnDelete;
        }

        private void UnbindNode(Node node) {
            node.OnMoved -= Node_OnMoved;
            node.OnClicked -= Node_OnClicked;
            node.OnDelete -= Node_OnDelete;
        }
    }
}