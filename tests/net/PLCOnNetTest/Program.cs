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

using MASES.PLCOnNetTest.Common;
using Org.Apache.Plc4x.JavaNs.Api;
using System;

namespace MASES.PLCOnNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting PLCOnNetTest");

            Initialize();

            ListProtocols();

            // TO BE COMPLETED
        }

        static void Initialize()
        {
            try
            {
                PLCOnNetTestCore.ApplicationHeapSize = "4G";
                PLCOnNetTestCore.ApplicationInitialHeapSize = "1G";
                PLCOnNetTestCore.CreateGlobalInstance();
                var appArgs = PLCOnNetTestCore.FilteredArgs;

                Console.WriteLine($"Initialized PLCOnNetTestCore, remaining arguments are {string.Join(" ", appArgs)}");
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
