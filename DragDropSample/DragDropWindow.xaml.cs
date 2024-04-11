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
using System.Windows.Shapes;

namespace DragDropSample
{
    /// <summary>
    /// DragDropWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DragDropWindow : Window
    {
        //鼠标是否按下
        private bool _isMouseDown = false;
        //鼠标按下的位置
        private Point _mouseDownPosition;
        //鼠标按下控件的位置
        private Point _mouseDownControlPosition;

        public DragDropWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var c = ((Control)sender);
            _isMouseDown = true;
            _mouseDownPosition = e.GetPosition(this);
            var transform = c.RenderTransform as TranslateTransform;
            if (transform == null)
            {
                transform = new TranslateTransform();
                c.RenderTransform = transform;
            }
            _mouseDownControlPosition = new Point(transform.X, transform.Y);
            c.CaptureMouse();
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                var c = sender as Control;
                var pos = e.GetPosition(this);
                var dp = pos - _mouseDownPosition;
                var transform = c.RenderTransform as TranslateTransform;
                transform.X = _mouseDownControlPosition.X + dp.X;
                transform.Y = _mouseDownControlPosition.Y + dp.Y;
            }
        }

        /// <summary>
        /// 鼠标按起释放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var c = sender as Control;
            _isMouseDown = false;
            c.ReleaseMouseCapture();
        }

        /// <summary>
        /// 复制控件触发拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Button curButton = (Button)sender;
            Button button = new Button();
            button.Width = curButton.Width;
            button.Height = curButton.Height;
            button.Content = curButton.Content;
            button.Style = curButton.Style;
            button.PreviewMouseDown += OnMouseDown;
            button.PreviewMouseMove += OnMouseMove;
            button.PreviewMouseUp += OnMouseUp;
            Point curMousePosition = e.GetPosition(this);
            Canvas.SetLeft(button, curMousePosition.X);
            Canvas.SetTop(button, curMousePosition.Y);
            button.Focus();
            CanvasToolBar.Children.Add(button);
            //CanvasToolBar.UpdateLayout();
            OnMouseDown(button, e);
        }
    }
}
