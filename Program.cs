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
            //Отмена задачи, как правило, выполняется следующим образом. Сначала  
            //получается признак отмены из источника признаков отмены.
            //Затем этот признак  передается задаче, после чего она должна контролировать
            //его на предмет получения запроса на отмену.
            //Создаем обьект источника признаков отмены у которого есть свойство public CancellationToken Token { get; }
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            // передаем обьект состояние от свойства Token класса CancellationTokenSource признаку отмены
            CancellationToken token = cancelTokenSource.Token;

            Action action = delegate {

                for (int i = 0; i < 100; i++)
                {
                    // проверяем наличие сигнала отмены задачи
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Операция прервана");
                        //  выходим из метода и тем самым завершаем задачу
                        return; 

                    }
                    Console.WriteLine($"Квадрат числа задачи Task {i} равен {i * i}");
                    Thread.Sleep(500);

                }


            };
            Task task = new Task(action,token);

            task.Start();
            // после задержки по времени отменяем выполнение задачи
            //Thread.Sleep(1000);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("итерация в основном потоке "+ i);
                Thread.Sleep(500); 

            }



            cancelTokenSource.Cancel();
            // ожидаем завершения задачи
            Thread.Sleep(1000);
            //  проверяем статус задачи
            Console.WriteLine($"Task Status: {task.Status}");
            cancelTokenSource.Dispose(); // освобождаем ресурсы


            CancellationTokenSource cancelTokenSource2 = new CancellationTokenSource();
            CancellationToken token2 = cancelTokenSource2.Token;

            Console.WriteLine("Выполнение другой задачи");
            Func<object, int> func = delegate (object v)
            {
                int x = (int)v;
                int sum = 0;

                for (; x > 0; x--) 
                {

                    if (token2.IsCancellationRequested) {
                        Console.WriteLine("Операция прервана");
                        return sum;
                    }
                    sum += x;
                    Thread.Sleep(500);
                    Console.WriteLine(sum + " " + x);

                }
                
                return sum;
            };
            Task<int> tsk2= new Task<int>(func,9,token2);
            tsk2.Start();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("итерация в основном потоке " + i);
                Thread.Sleep(500);

            }
            cancelTokenSource2.Cancel();    
            tsk2.Wait();
            //  проверяем статус задачи
            Console.WriteLine($"Task Status: {tsk2.Status}");
            cancelTokenSource2.Dispose(); // освобождаем ресурсы


        }
    }
}


//// Сконструировать и запустить  объект  задачи tsk. 
//// метод StartNew(MyTask) использует делегат: StartNew(Func<TResult> function)
//// ,который соответствует(делегат - Func<TResult> function ) методу MyTask;
//// В том числе обращение к методу StartNew класса TaskFactory происходит
//// через свойство(Factory) класса  Task<TResult>, т.к. свойство имеет тип TaskFactory<TResult> Factory { get; }
//Task<bool> tsk = Task<bool>.Factory.StartNew(MyTask);
//// наглядное получение(чтение) результата задачи  
//bool b = tsk.Result;
//Console.WriteLine("Результат после выполнения задачи MyTask: " + tsk.Result);

////использование самого делегат(tskf)
//Func<bool> tskf = delegate { return false; };
//tsk = Task<bool>.Factory.StartNew(tskf);
//Console.WriteLine("Результат после выполнения задачи Func<bool> tskf: " + tsk.Result);
//tsk.Dispose();
//Console.WriteLine("***");
////запуск через метод Run
//tsk = Task<bool>.Run<bool>(delegate { return true; });
//Console.WriteLine(tsk.Result);
//tsk.Dispose();
//tsk = Task<bool>.Run<bool>(() => false);
//Console.WriteLine(tsk.Result);
//tsk.Dispose();
//Console.WriteLine("***");
////стандартный запуск задачи
//Task<bool> tsk4 = new Task<bool>(tskf);
//tsk4.Start();
//tsk4.Wait();
//Console.WriteLine("Результат стандартного запуска задачи:" + tsk4.Result + " через Start");








////Работа Action делегата
//Action action = delegate { Console.WriteLine("Делегат Action"); };
//Task task5 = new Task(action);
//task5.Start();
//task5.Wait();

//task5.Dispose();
//// Запуск задачи  используя статического метода Run
//Task.Run(action);


//CancellationTokenSource cansel = new CancellationTokenSource();
//CancellationToken token = new CancellationToken();
//token = cansel.Token;


//Action<object> action2 = delegate (object o) {

//    Console.WriteLine(o);
//};
//Task tsk_obj = new Task(action2, "Dron");
//tsk_obj.Start();
//tsk_obj.Wait();
//tsk_obj.Dispose();



//// Сконструировать объект  задачи tsk2. 
//Task<int> tsk2 = Task<int>.Factory.StartNew(Sumlt, 3);
////получим результат задачи
//Console.WriteLine("Результат после выполнения задачи Sumlt: " + tsk2.Result);
////непосредственно через делегат
//Func<object, int> func = delegate (object v)
//{
//    int x = (int)v;
//    int sum = 0;
//    for (; x > 0; x--)
//        sum += x;
//    Console.WriteLine("77");
//    return sum;


//};
//int x = func(6);

//tsk2 = Task<int>.Factory.StartNew(func, 4);

//Console.WriteLine("***");
////возвращает объект состояния задачи
//Console.WriteLine(tsk2.AsyncState);
////возвращает true, если задача была отменена
//Console.WriteLine(tsk2.IsCanceled);
////возвращает true, если задача завершена
//Console.WriteLine(tsk2.IsCompleted);
////возвращает статус задачи
//Console.WriteLine(tsk2.Status);
//Console.WriteLine("***");

//Console.WriteLine("Результат после выполнения задачи  Func<object,int> func: " + tsk2.Result);
//tsk2.Dispose();





//Func<object, int> func1 = delegate (object i)
//{

//    return (int)i;
//};
//конструирование задачи через конструктор и методы
//Task<int> tsk3 = new Task<int>(func1, 6);
//tsk3.Start();
//tsk3.Wait();
//Console.WriteLine(tsk3.Result + " результат");
//tsk3.Dispose();

//Action<object> action2 = delegate (object i) { Console.WriteLine(i); };
//Task task4 = new Task(action2, "drpon");
//task4.Start();
//task4.Wait();
//task4.Dispose();


//Action action3 = delegate () { Console.WriteLine("action3"); };
//Task.Run(action3);

//1 В классе Task<TResult> обращаемся к статическому свойству Factory с возвращаемым типом TaskFactory
//2 Через него(свойство Factory) обращаемся или читаем класс TaskFactory, а именно Task<TResult> StartNew(Func<object?, TResult> function, object? state);
//3 В метод(StartNew) помещаем аргументы: делегат(Func<object, int> func1) и его обьект для делегата           
//tsk3 = Task<int>.Factory.StartNew(func1, 5);
//и получаем результат
//Console.WriteLine(tsk3.Result + " результат");
//удаляем обьект
//tsk3.Dispose();