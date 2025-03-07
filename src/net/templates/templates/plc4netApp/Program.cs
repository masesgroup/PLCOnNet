using MASES.PLC4Net;
using System;

namespace MASES.PLC4NetTemplate.PLC4NetApp
{
    class Program
    {
        class MyPLC4NetCore : PLC4NetCore<MyPLC4NetCore> { }

        static void Main(string[] _)
        {
            MyPLC4NetCore.CreateGlobalInstance(); // this call prepares the environment: it is mandatory to initialize the JVM
            var appArgs = MyPLC4NetCore.FilteredArgs; // contains the remaining arguments: the PLC4Net, JNet and JCOBridge arguments are discarded
            if (appArgs.Length != 0)
            {


                Console.WriteLine($"Opening {appArgs[0]}");
            }
        }
    }
}
