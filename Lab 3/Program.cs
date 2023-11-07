using System;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace Lab3
{
    internal class Program
    {
        public static object TryParse { get; private set; }

        public static void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            Tuple<iFunction, double, double, double> arguments = (Tuple<iFunction, double, double,double>)e.Argument;

            iFunction function = arguments.Item1;
            double step = arguments.Item2;
            double range_start = arguments.Item3;
            double range_end = arguments.Item4;

            step = 1/step;

            double[] score = new double[2];
            int totalIterations = (int)((range_end - range_start) / step);

            for (int i = 0; i <= totalIterations; i++)
            {
                double x = range_start + i * step;
                score[0] += (function.Evaluate(x) + function.Evaluate(x + step)) * step / 2;

                score[1] += (function.Evaluate(x) * step);

                //postep
                if (i % (totalIterations / 10) == 0)
                {
                    int progressPercentage = (int)((i / (double)totalIterations) * 100);
                    worker.ReportProgress(progressPercentage);
                }
            }

            e.Result = score;
        }


        public static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e, double range_start, double range_end)
        {
            Console.WriteLine($"Postęp obliczeń metodą trapezów dla przedziału({range_start},{range_end}): {e.ProgressPercentage}%");
        }

        public static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, double range_start, double range_end)
        {
            if (e.Error != null)
            {
                Console.WriteLine($"Błąd: {e.Error.Message}");
            }
            else if (e.Cancelled)
            {
                Console.WriteLine("Przerwano obliczenia");
            }
            else
            {

                double[] score;
                score = (double[])e.Result;
                string result = $"|Wynik obliczeń metodą trapezów dla przedziału({range_start},{range_end}) : {Math.Round(score[0], 3)}|";
                string result2 = $"|Wynik obliczeń metodą prostokątów dla przedziału({range_start},{range_end}) : {Math.Round(score[1], 3)}|";

                score[1] = Math.Round(score[1], 3);
                Console.WriteLine("|"+new string('-', result2.Length)+"|");
                Console.WriteLine(result);
                Console.WriteLine("|"+new string('.', result2.Length)+"|");
                Console.WriteLine(result2);
                Console.WriteLine("|"+new string('-', result2.Length)+"|");


            }
        }

        public static void Calculation(iFunction function,double steps, double range_start, double range_end)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.ProgressChanged += (sender, e) => worker_ProgressChanged(sender, e, range_start, range_end);
            worker.RunWorkerCompleted += (sender, e) => worker_RunWorkerCompleted(sender, e, range_start, range_end);

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            Tuple<iFunction, double, double, double> arguments = new Tuple<iFunction, double, double, double>(function, steps, range_start, range_end);
            worker.RunWorkerAsync(arguments);
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double function_pick = 0;
            double steps =0;

            do
            {
                Console.WriteLine("Wybierz funkcję do obliczeń:");
                Console.WriteLine("1. y=2x+2x^2");
                Console.WriteLine("2. y=2x^2");
                Console.WriteLine("3. y=2x-3");
                Console.WriteLine("4. y=2x^3-3x^2+4x-1");
                Console.WriteLine("5. y=x^3-6x^2+11x-6");
                Console.WriteLine("6. y=3x^3-5x^2+2x+8");

                Console.WriteLine("0. Wyjdź");

                if (!double.TryParse(Console.ReadLine(), out function_pick) || function_pick < 0 || function_pick > 6)
                {
                    Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                    continue;
                }

                if (function_pick == 0) break;

                iFunction function = null;

                switch (function_pick)
                {
                    case 1:
                        function = new Functions.Function1();
                        break;
                    case 2:
                        function = new Functions.Function2();
                        break;
                    case 3:
                        function = new Functions.Function3();
                        break;
                    case 4:
                        function = new Functions.Function4();
                        break;
                    case 5:
                        function = new Functions.Function5();
                        break;
                    case 6:
                        function = new Functions.Function6();
                        break;

                }

                Console.WriteLine("Wprowadz liczbę kroków (dokładność obliczeń)");
                steps = double.Parse(Console.ReadLine());
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Wciśnij dowolny przycisk aby zakończyć obliczenia");
                Console.WriteLine("");



                Calculation(function, steps, -10, 10);
                Calculation(function, steps, -5, 20);
                Calculation(function, steps, -5, 0);

                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;
                Console.WriteLine($"Czas wykonania: {elapsed}");

                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();

            } while (function_pick != 0);
        }
    }
}
