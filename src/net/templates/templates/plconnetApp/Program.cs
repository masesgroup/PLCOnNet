using Java.Time;
using Java.Util.Concurrent;
using Java.Util.Function;
using MASES.PLCOnNet;
using MASES.PLCOnNet.Specific;
using Org.Apache.Plc4x.JavaNs.Api;
using Org.Apache.Plc4x.JavaNs.Api.Messages;
using Org.Apache.Plc4x.JavaNs.Api.Model;
using Org.Apache.Plc4x.JavaNs.Api.Types;
using System;
using System.Threading;

namespace MASES.PLCOnNetTemplate.PLCOnNetApp
{
    class Program
    {
        class MyPLCOnNetCore : PLCOnNetCore<MyPLCOnNetCore> { }

        private static readonly bool _useAsync = false;
        static Consumer<PlcSubscriptionEvent> _plcEvent = null;

        static void Main(string[] _)
        {
            MyPLCOnNetCore.CreateGlobalInstance(); // this call prepares the environment: it is mandatory to initialize the JVM
            var appArgs = MyPLCOnNetCore.FilteredArgs; // contains the remaining arguments: the PLCOnNet, JNet and JCOBridge arguments are discarded
            if (appArgs.Length != 0)
            {
                // copied from https://plc4x.apache.org/plc4x/latest/users/getting-started/plc4j.html
                Console.WriteLine($"Opening connection to {appArgs[0]}");

                using var plcConnection = PlcDriverManager.Default.ConnectionManager.GetConnection(appArgs[0]);

                ReadRequest(plcConnection);
                WriteRequest(plcConnection);
                SubscriptionRequest(plcConnection);
            }
        }

        static void ReadRequest(PlcConnection plcConnection)
        {
            if (!plcConnection.Metadata.IsReadSupported())
            {
                Console.WriteLine("This connection doesn't support reading.");
                return;
            }
            // Create a new read request:
            // - Give the single item requested an alias name
            PlcReadRequest.Builder builder = plcConnection.ReadRequestBuilder();
            builder.AddTagAddress("value-1", "%Q0.4:BOOL");
            builder.AddTagAddress("value-2", "%Q0:BYTE");
            builder.AddTagAddress("value-3", "%I0.2:BOOL");
            builder.AddTagAddress("value-4", "%DB.DB1.4:INT");
            PlcRequest readRequest = builder.Build();

            if (_useAsync)
            {
                readRequest.RequestAsync<PlcReadResponse>(ProcessResponse).Wait();
            }
            else
            {
                readRequest.Request<PlcReadResponse>(ProcessResponse);   
            }
        }

        static void WriteRequest(PlcConnection plcConnection)
        {
            if (!plcConnection.Metadata.IsWriteSupported())
            {
                Console.WriteLine("This connection doesn't support writing.");
                return;
            }
            // Create a new write request:
            // - Give the single item requested an alias name
            // - Pass in the data you want to write (for arrays, pass in one value for every element)
            PlcWriteRequest.Builder builder = plcConnection.WriteRequestBuilder();
            builder.AddTagAddress("value-1", "%Q0.4:BOOL", true);
            builder.AddTagAddress("value-2", "%Q0:BYTE", (byte)0xFF);
            builder.AddTagAddress("value-4", "%DB.DB1.4:INT[3]", 7, 23, 42);
            PlcRequest writeRequest = builder.Build();

            if (_useAsync)
            {
                writeRequest.RequestAsync<PlcWriteResponse>(ProcessResponse).Wait();
            }
            else
            {
                writeRequest.Request<PlcWriteResponse>(ProcessResponse);
            }
        }

        static void SubscriptionRequest(PlcConnection plcConnection)
        {
            if (!plcConnection.Metadata.IsSubscribeSupported())
            {
                Console.WriteLine("This connection doesn't support subscribing.");
                return;
            }
            // Create a new subscription request:
            // - Give the single tag requested an alias name
            PlcSubscriptionRequest.Builder builder = plcConnection.SubscriptionRequestBuilder();
            builder.AddChangeOfStateTagAddress("value-1", "{some address}");
            builder.AddCyclicTagAddress("value-2", "{some address}", Duration.OfMillis(1000));
            builder.AddEventTagAddress("value-3", "{some alarm address}");
            PlcRequest subscriptionRequest = builder.Build();

            if (_useAsync)
            {
                subscriptionRequest.RequestAsync<PlcSubscriptionResponse>(ProcessResponse).Wait();
            }
            else
            {
                subscriptionRequest.Request<PlcSubscriptionResponse>(ProcessResponse);
            }
        }

        static void ProcessResponse(PlcReadResponse response)
        {
            foreach (Java.Lang.String tagName in response.TagNames)
            {
                if (response.GetResponseCode(tagName) == PlcResponseCode.OK)
                {
                    int numValues = response.GetNumberOfValues(tagName);
                    // If it's just one element, output just one single line.
                    if (numValues == 1)
                    {
                        Console.WriteLine($"Value[{tagName}]: {response.GetObject(tagName)}");
                    }
                    // If it's more than one element, output each in a single row.
                    else
                    {
                        Console.WriteLine($"Value[{tagName}]:");
                        for (int i = 0; i < numValues; i++)
                        {
                            Console.WriteLine($" - {response.GetObject(tagName, i)}");
                        }
                    }
                }
                // Something went wrong, to output an error message instead.
                else
                {
                    Console.WriteLine($"Error[{tagName}]: {response.GetResponseCode(tagName).Name()}");
                }
            }
        }

        static void ProcessResponse(PlcWriteResponse response)
        {
            foreach (Java.Lang.String tagName in response.TagNames)
            {
                if (response.GetResponseCode(tagName) == PlcResponseCode.OK)
                {
                    Console.WriteLine($"Value[{tagName}]: updated");
                }
                // Something went wrong, to output an error message instead.
                else
                {
                    Console.WriteLine($"Error[{tagName}]: {response.GetResponseCode(tagName).Name()}");
                }
            }
        }

        static void ProcessEvent(PlcSubscriptionEvent e)
        {
            foreach (Java.Lang.String tagName in e.TagNames)
            {
                Console.WriteLine(e.GetPlcValue(tagName));
            }
        }

        static void ProcessResponse(PlcSubscriptionResponse response)
        {
            _plcEvent ??= new Consumer<PlcSubscriptionEvent>()
            {
                OnAccept = ProcessEvent
            };

            foreach (PlcSubscriptionHandle subscriptionHandle in response.SubscriptionHandles)
            {
                subscriptionHandle.Register(_plcEvent);
            }
        }
    }
}
