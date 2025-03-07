/*
*  Copyright 2025 MASES s.r.l.
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*  http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*
*  Refer to LICENSE for more information.
*/

using MASES.PLC4NetTest.Common;
using Org.Apache.Plc4x.JavaNs.Api;
using System;

namespace MASES.PLC4NetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting PLC4NetTest");

            Initialize();

            ListProtocols();

            // TO BE COMPLETED
        }

        static void Initialize()
        {
            try
            {
                PLC4NetTestCore.ApplicationHeapSize = "4G";
                PLC4NetTestCore.ApplicationInitialHeapSize = "1G";
                PLC4NetTestCore.CreateGlobalInstance();
                var appArgs = PLC4NetTestCore.FilteredArgs;

                Console.WriteLine($"Initialized PLC4NetTestCore, remaining arguments are {string.Join(" ", appArgs)}");
            }
            catch (Java.Lang.Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        static void ListProtocols()
        {
            Console.WriteLine("Managed protocols are:");
            var codes = PlcDriverManager.Default.ProtocolCodes;
            foreach (var item in codes)
            {
                Console.WriteLine(item);
            }
        }

    }
}
