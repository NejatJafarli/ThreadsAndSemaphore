using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ThreadsAndSemaphore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int THREADCOUNT = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        public ObservableCollection<string> FakeTheads { get; set; }=new ObservableCollection<string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            THREADCOUNT++;
            FakeTheads.Add($"Thread {THREADCOUNT}");
        }
    }
}
