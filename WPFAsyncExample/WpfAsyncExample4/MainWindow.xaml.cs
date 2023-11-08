using System.Diagnostics;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace WpfAsyncExample4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch sw = new Stopwatch();
        private double ArriveTime => Math.Round(sw.ElapsedMilliseconds / 1000d, 2);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start()
        {
            sw.Start();
            ClearLogs();
            ResetBugs();
        }

        private void End()
        {
            AddLogs($"총 소요시간은: {ArriveTime} 초");
            sw.Stop();
            sw.Reset();
        }

        private void AddLogs(params string[] texts)
        {
            foreach (string text in texts)
            {
                lbLogs.Items.Add(text);
            }
        }

        private void ClearLogs() => lbLogs.Items.Clear();

        private void ResetBugs()
        {
            var bugs = new List<Label>() { bugA, bugB, bugC, bugD };
            foreach (var label in bugs)
            {
                label.Margin = new Thickness(140, label.Margin.Top, 0, 0);
            }
        }

        private async Task<string> MoveLabel(Label lbl, int speed)
        {
            double endX = finishLine.Margin.Left - lbl.ActualWidth;
            while (lbl.Margin.Left < endX)
            {
                double currentLeft = lbl.Margin.Left;
                lbl.Margin = new Thickness(currentLeft + speed * 4, lbl.Margin.Top, 0, 0);
                await Task.Delay(1);
            }
            lbl.Margin = new Thickness(endX, lbl.Margin.Top, 0, 0);

            return $"{lbl.Content} 도착시간: {ArriveTime} 초";
        }

        private async void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            Start();

            string logA = await MoveLabel(bugA, 1);
            string logB = await MoveLabel(bugB, 2);
            string logC = await MoveLabel(bugC, 3);
            string logD = await MoveLabel(bugD, 4);

            AddLogs(logA, logB, logC, logD);

            End();
        }

        private async void btnSecond_Click(object sender, RoutedEventArgs e)
        {
            Start();

            Task<string> bugATask = MoveLabel(bugA, 1);
            Task<string> bugBTask = MoveLabel(bugB, 2);

            //string logA = await bugATask;
            //string logB = await bugBTask;
            //AddLogs(logA, logB);

            string[] results = await Task.WhenAll(bugATask, bugBTask); // A, B 2개의 Task 가 모두 끝나여 결과값 Collection 형태로 반환
            AddLogs(results);

            Task<string> bugCTask = MoveLabel(bugC, 3);
            Task<string> bugDTask = MoveLabel(bugD, 4);

            //string logC = await bugCTask;
            //string logD = await bugDTask;
            //AddLogs(logC, logD);

            results = await Task.WhenAll(bugCTask, bugDTask);
            AddLogs(results);

            End();
        }

        private async void btnThird_Click(object sender, RoutedEventArgs e)
        {
            Start();

            Task<string> bugATask = MoveLabel(bugA, 1);
            Task<string> bugBTask = MoveLabel(bugB, 2);
            Task<string> bugCTask = MoveLabel(bugC, 3);
            Task<string> bugDTask = MoveLabel(bugD, 4);

            string logA = await bugATask;
            string logB = await bugBTask;
            string logC = await bugCTask;
            string logD = await bugDTask;

            AddLogs(logA, logB, logC, logD);

            End();
        }

        private async void btnFourth_Click(object sender, RoutedEventArgs e)
        {
            Start();

            Task<string> bugATask = MoveLabel(bugA, 1);
            Task<string> bugBTask = MoveLabel(bugB, 2);
            Task<string> bugCTask = MoveLabel(bugC, 3);
            Task<string> bugDTask = MoveLabel(bugD, 4);

            var tasks = new List<Task>() { bugATask, bugBTask, bugCTask, bugDTask };

            while (tasks.Count > 0)
            {
                Task task = await Task.WhenAny(tasks); // 리스트에 담은 Task 중, 한개라도 작업이 끝나면 작업 반환
   
                string log = await(Task<string>)task;
                AddLogs(log);
                tasks.Remove(task);
            }

            End();
        }
    }
}
