using System;
using System.Threading.Tasks;
using System.Threading;

namespace Console_delegate
{



    internal class Program
    {

        static bool MyTask()
        {
            return true;
        }

        static int Sumlt(object v)
        {
            int x = (int)v;
            int sum = 0;
            for (; x > 0; x--)
                sum += x;
            return sum;
        }



        static void Main(string[] args)
        {

            Console.WriteLine("Основной поток запущен.");

            // Сконструировать объект первой задачи. 
            Task<bool> tsk = Task<bool>.Factory.StartNew(MyTask);
            Console.WriteLine("Результат после выполнения задачи MyTask: " + tsk.Result);

            // Сконструировать объект второй задачи. 
            Task<int> tsk2 = Task<int>.Factory.StartNew(Sumlt, 3);
            Console.WriteLine("Результат после выполнения задачи Sumlt: " + tsk2.Result);
            object v = tsk.Result; 
           

        }
    }
}
