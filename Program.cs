using System;
using System.Threading.Tasks;
using System.Threading;

namespace Console_delegate
{



    internal class Program
    {

        static bool MyTask()
        {
            Console.WriteLine("задача MyTask");
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

            // Сконструировать и запустить  объект  задачи tsk. 
            // метод StartNew(MyTask) использует делегат: StartNew(Func<TResult> function)
            // ,который соответствует(делегат - Func<TResult> function ) методу MyTask;
            // В том числе обращение к методу StartNew класса TaskFactory происходит
            // через свойство(Factory) класса  Task<TResult>, т.к. свойство имеет тип TaskFactory<TResult> Factory { get; }
            Task<bool> tsk = Task<bool>.Factory.StartNew(MyTask);
            // наглядное получение(чтение) результата задачи  
            bool b = tsk.Result;
            Console.WriteLine("Результат после выполнения задачи MyTask: " + tsk.Result);          

            //использование самого делегат(tskf)
            Func<bool> tskf = delegate { return false; };
            tsk = Task<bool>.Factory.StartNew(tskf);
            Console.WriteLine("Результат после выполнения задачи Func<bool> tskf: " + tsk.Result);
            tsk.Dispose();
            Console.WriteLine("***");
            tsk = Task<bool>.Run<bool>(delegate { return true; });
            Console.WriteLine(tsk.Result);
            tsk.Dispose();
            tsk = Task<bool>.Run<bool>(() => false) ;
            Console.WriteLine(tsk.Result);
            tsk.Dispose();  
            Console.WriteLine("***");
            //стандартный запуск задачи
            Task<bool> tsk4 = new Task<bool>(tskf);
            tsk4.Start();
            tsk4.Wait();
            Console.WriteLine("Результат стандартного запуска задачи:" + tsk4.Result + " через Start");

            //Работа Action делегата
            Action action = delegate { Console.WriteLine("Делегат Action"); };
            Task task5 = new Task(action);
            task5.Start();  
            task5.Wait();

            task5.Dispose();
            // Запуск задачи  используя статического метода Run
            Task.Run(action);





            Action<object> action2 = delegate (object o) {
                 
                Console.WriteLine(o); };
            Task tsk_obj = new Task(action2,"Dron");
            tsk_obj.Start();
            tsk_obj.Wait();
            tsk_obj.Dispose();
            


            // Сконструировать объект  задачи tsk2. 
            Task<int> tsk2 = Task<int>.Factory.StartNew(Sumlt, 3);
            //получим результат задачи
            Console.WriteLine("Результат после выполнения задачи Sumlt: " + tsk2.Result);
            //непосредственно через делегат
            Func<object,int> func = delegate(object v)
            {
                int x = (int)v;
                int sum = 0;
                for (; x > 0; x--)
                    sum += x;
                Console.WriteLine("77");
                return sum;
             

            };
            int x = func(6);    

            tsk2 = Task<int>.Factory.StartNew(func,4);

            Console.WriteLine("***");
            //возвращает объект состояния задачи
            Console.WriteLine(tsk2.AsyncState);
            //возвращает true, если задача была отменена
            Console.WriteLine(tsk2.IsCanceled);
            //возвращает true, если задача завершена
            Console.WriteLine(tsk2.IsCompleted);
            //возвращает статус задачи
            Console.WriteLine(tsk2.Status);
            Console.WriteLine("***");

            Console.WriteLine("Результат после выполнения задачи  Func<object,int> func: " + tsk2.Result);
            tsk2.Dispose();



            //Func<Task<int>> function = delegate { return tsk2; };
            //Console.WriteLine("***");
            //Task<int>.Run<int>(function);
            //Console.WriteLine("***");



            Task tsk3 = Task.Run(delegate { Console.WriteLine("Run"); });

            Func<Task> f = delegate { return /*tsk2 = */Task<int>.Factory.StartNew(func,6); };
            Console.WriteLine(f);


            //Func<Func<Task<int>>,int> func4 =  delegate(Func<Task<int>> func1)
            //{
            //    func1 = delegate { return /*tsk2 =*/ Task<int>.Factory.StartNew(func,4); };
            //    func1.Invoke();
            //    return 5;

            //};
            //int x2 = func4(f);
            //Console.WriteLine(x2);


            Func<int, Func<Task<int>>,Task> func5 = delegate (int i, Func<Task<int>> func1)
            {
                ++i;
                func1 = delegate { return /*tsk2 =*/ Task<int>.Factory.StartNew(func, 4); };
                func1.Invoke();
                return tsk3;

            };
            Console.WriteLine(func5);




            //Func<Func<Task<int>>, int> func6 = delegate () { return 4; };
            //Task task4 = Task.Factory.StartNew(func4);
            //task4.Wait();  

        }
    }
}
