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

namespace Test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Vector relativeMousePos;
        FrameworkElement draggedObject;
        public MainWindow()
        {
            InitializeComponent();
        }
        public bool p = true;
        void StartDrag(object sender, MouseButtonEventArgs e) //MouseLeftButtonDown="StartDrag"/>
        {
            draggedObject = (FrameworkElement)sender;
            
            relativeMousePos = e.GetPosition(draggedObject) - new Point();
             draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void UpdatePosition(MouseEventArgs e)
        {
            //var point = e.GetPosition(DragArena);
            var newPos =/* point - */relativeMousePos;
            Canvas.SetLeft(draggedObject, newPos.X);
            Canvas.SetTop(draggedObject, newPos.Y);
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            draggedObject.MouseMove -= OnDragMove;
            draggedObject.LostMouseCapture -= OnLostCapture;
            draggedObject.MouseUp -= OnMouseUp;
            UpdatePosition(e);
        }



        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Image pic = new Image();
            pic.Source =  new BitmapImage(new Uri("Images/onr.jpg", UriKind.Relative));
            MyGrid.Children.Add(pic);
            Grid.SetRow(pic, 3);
            Grid.SetColumn(pic, 3);
        }

        private void drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartDrag(sender, e);
        }
    }
}
