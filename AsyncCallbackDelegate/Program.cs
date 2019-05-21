using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace AsyncCallbackDelegate
{
    public delegate int BinaryOp(int x, int y);
    class Program
    {
        public static bool isDone = false;
        static void Main(string[] args)
        {
            Console.WriteLine("ASyncCallbackDelegate Example");
            //Вывести идентификатор выполняющегося потока
            Console.WriteLine($"Main() invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            //Вызвать Add() во вторичном потоке
            BinaryOp b = new BinaryOp(Add);
            IAsyncResult ar = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), "Main() thanks you fo adding these numbers!");
            //Это сообщение продолжит выводиться до тех пор, пока не будет завершен метод Add()
            while (!isDone)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Working...");
            }

            Console.ReadLine();
        }
        static int Add(int x, int y)
        {
            //Вывести идентификатор выполняющегося потока
            Console.WriteLine($"Add() invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            //Сделать паузу для моделирования длительной операции.
            Thread.Sleep(5000);
            return x + y;
        }

        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine("AddComplete() invoked one thread {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete");
            //теперь получить результат
            AsyncResult ar = (AsyncResult)iar;
            string message = (string)iar.AsyncState;
            Console.WriteLine(message);
            BinaryOp b = (BinaryOp)ar.AsyncDelegate;
            Console.WriteLine($"10 + 10 is {b.EndInvoke(iar)}");
            isDone = true;
        }
    }
}
