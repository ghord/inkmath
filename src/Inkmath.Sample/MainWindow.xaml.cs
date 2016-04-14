using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inkmath.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IncrementalAnalyzer analyzer_ = new IncrementalAnalyzer(new AnalyzerConfiguration("seshat.exe", @"..\..\seshat_config\CONFIG"));

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            analyzer_.AddStroke(e.Stroke);
        }

        private void InkCanvas_StrokeErased(object sender, RoutedEventArgs e)
        {
            //TODO: stroke removal
         
        }

        public IncrementalAnalyzer Analyzer => analyzer_;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ink.Strokes.Clear();
            analyzer_.Clear();
        }
    }
}
