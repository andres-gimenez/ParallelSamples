using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsParallel
{
    public partial class Form1 : Form
    {
        private readonly SynchronizationContext SyncContext;

        public Form1()
        {
            InitializeComponent();

            SyncContext = AsyncOperationManager.SynchronizationContext;
        }

        private void butStart0_Click(object sender, EventArgs e)
        {
            var prBars = new[] { prBar1, prBar2, prBar3, prBar4,
                    prBar5, prBar6, prBar7, prBar8, prBar9, prBar10,
                    prBar11, prBar12, prBar13, prBar14, prBar15, prBar16 };

            for (int i = 0; i <= 100; i++)
            {
                foreach (var prbar in prBars)
                {
                    Task.Delay(TimeSpan.FromSeconds(1));
                    prbar.Value = i;
                }
            }
        }

        private void butStart1_Click(object sender, EventArgs e)
        {
            var prBars = new[] { prBar1, prBar2, prBar3, prBar4,
                    prBar5, prBar6, prBar7, prBar8, prBar9, prBar10,
                    prBar11, prBar12, prBar13, prBar14, prBar15, prBar16 };

            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    Parallel.ForEach(prBars, (prbar) =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(1));
                        prbar.Value = i;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butStart2_Click(object sender, EventArgs e)
        {
            var prBars = new[] { prBar1, prBar2, prBar3, prBar4,
                    prBar5, prBar6, prBar7, prBar8, prBar9, prBar10,
                    prBar11, prBar12, prBar13, prBar14, prBar15, prBar16 };

            try
            {
                foreach (var prbar in prBars)
                {
                    var result = Parallel.For(1, 101, i =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(1));
                        prbar.Value = i;
                    });

                    //MessageBox.Show(result.IsCompleted.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butStart3_Click(object sender, EventArgs e)
        {
            var prBars = new[] { prBar1, prBar2, prBar3, prBar4,
                    prBar5, prBar6, prBar7, prBar8, prBar9, prBar10,
                    prBar11, prBar12, prBar13, prBar14, prBar15, prBar16 };
            var range = Enumerable.Range(1, 100).ToArray();
            var tareas = prBars.Join(range, x => 0, y => 0, (bar, rangeValue) => (bar, rangeValue)).ToArray();

            try
            {
                var result = Parallel.ForEach(tareas, tarea =>
                {
                    try
                    {
                        Task.Delay(100);
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                            Debug.WriteLine($"+{tarea.bar.Name} -> {tarea.rangeValue}");

                            //SyncContext.Post(e =>
                            //{
                            //    try
                            //    {
                                    Debug.WriteLine($"*{tarea.bar.Name} -> {tarea.rangeValue}");
                                    tarea.Item1.Value = tarea.rangeValue;
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        MessageBox.Show(ex.Message);
                            //    }
                            //}, null);

                            //this.Invoke(new MethodInvoker(delegate
                            //{
                            //try
                            //{
                            //tarea.Item1.Value = tarea.Item2;
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    MessageBox.Show(ex.Message);
                                    //}
                                //}));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

                //MessageBox.Show(result.IsCompleted.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void ButStart4_Click(object sender, EventArgs e)
        {
            var prBars = new[] { prBar1, prBar2, prBar3, prBar4,
                    prBar5, prBar6, prBar7, prBar8, prBar9, prBar10,
                    prBar11, prBar12, prBar13, prBar14, prBar15, prBar16 };
            var range = Enumerable.Range(1, 100).ToArray();
            var tareas = prBars.Join(range, x => 0, y => 0, (bar, rangeValue) => (bar, rangeValue)).ToArray();

            var tasks = new List<Task>();
            foreach (var (bar, rangeValue) in tareas)
            {
                tasks.Add(Task.Run(() =>
                {
                    //this.Invoke(new Action(() =>
                    //{
                    //    Debug.WriteLine($"*{bar.Name} -> {rangeValue}");
                    //    bar.Value = rangeValue;
                    //}));

                    SyncContext.Post(e =>
                    {
                        Debug.WriteLine($"*{bar.Name} -> {rangeValue}");
                        bar.Value = rangeValue;
                    }, null);
                }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
