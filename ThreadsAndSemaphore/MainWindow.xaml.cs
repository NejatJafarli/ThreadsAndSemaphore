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
        public Semaphore MySemaphore { get; set; } = new Semaphore(1,1,"Sema");
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public List<Thread> Threads { get; set; } = new List<Thread>();
        public ObservableCollection<string> FakeTheads { get; set; } = new ObservableCollection<string>();

        static public int MyI { get; set; } = 1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var temp = new Thread(ThreadMethod);
            Threads.Add(temp);
            FakeTheads.Add($"Thread {temp.ManagedThreadId}");

        }
        public void ThreadMethod(object state)
        {
            bool st = false;
            while (!st)
            {

                if (MySemaphore.WaitOne(10))
                {
                    int second = 0;
                    int index = 0;
                    string item = "";
                    string CurrentThreadName = "";
                    object ThisThreadItem = null;
                    var ThreadId2 = Thread.CurrentThread.ManagedThreadId;
                    this.Dispatcher.Invoke(() =>
                    {
                        second = 2 + 2 * MyI++;
                        CurrentThreadName = $"Thread {ThreadId2}--> {second}";
                        index = LBWorkingTH.Items.Add(CurrentThreadName);
                        ThisThreadItem = LBWorkingTH.Items[index];
                        item = ThisThreadItem.ToString();
                        if (LBWaitingTH.Items.Contains($"Thread {ThreadId2}"))
                            LBWaitingTH.Items.Remove($"Thread {ThreadId2}");
                    });
                    try
                    {
                        for (int i = 0; i < second; i++)
                        {
                            string newText = "";
                            Thread.Sleep(1000);
                            var temp = item.ToString().Split(' ');
                            for (int k = 0; k < temp.Length - 1; k++)
                            {
                                newText += $"{temp[k]} ";
                            }
                            newText += (int.Parse(temp[temp.Length - 1]) - 1).ToString();
                            this.Dispatcher.Invoke(() =>
                            {
                                index = LBWorkingTH.Items.IndexOf(item);
                                LBWorkingTH.Items[index] = newText;
                                item = newText;
                            });
                        }
                    }
                    finally
                    {
                        st = true;
                        this.Dispatcher.Invoke(() =>
                        {
                            LBWorkingTH.Items.RemoveAt(index);
                        });
                        MySemaphore.Release();
                    }
                }
                else
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    this.Dispatcher.Invoke(() =>
                    {
                        if (!LBWaitingTH.Items.Contains($"Thread {threadId}"))
                            LBWaitingTH.Items.Add($"Thread {threadId}");
                    });
                    Thread.Sleep(1000);
                }
            }
        }

        private void LBCreatedTH_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var index = LBCreatedTH.SelectedIndex;
            var id = int.Parse(FakeTheads[index].ToString().Split(' ')[1]);
            FakeTheads.RemoveAt(index);

            Threads.Find(x=>x.ManagedThreadId==id).Start(MySemaphore);

        }

        private void ThreadCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int Value = (int)ThreadCount.Value;
            MySemaphore.Dispose();
            MySemaphore= new Semaphore(Value, Value, "Sema");

        }
    }
}
