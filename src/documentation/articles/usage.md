---
title: Usage of .NET suite for PLC4X™
_description: Describes how to use PLCOnNet, set-up environment, identify the JVM™ and write good code
---

# PLCOnNet usage

To use PLCOnNet classes the developer can write code in .NET using the same classes available in the official Java packages.
If classes or methods are not available yet it is possible to use the approach synthetized in [What to do if an API was not yet implemented](https://jnet.masesgroup.com/articles/API_extensibility.html)

The following code snippets and examples are mutuated from [PLC4X™ official website](https://plc4x.apache.org/plc4x/latest/users/getting-started/plc4j.html)

## Read data from PLC

The following code connects to a Siemens PLC using the _s7_ protocol and requests to read some tags:

``` C#

const string connectionString = "s7://10.10.64.20";

using var plcConnection = PlcDriverManager.Default.ConnectionManager.GetConnection(connectionString);

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

var cf = readRequest.Execute<PlcReadResponse>();
var response = cf.Get();

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

```

## Write data to PLC

The following code connects to a Siemens PLC using the _s7_ protocol and requests to write some tags:

``` C#

const string connectionString = "s7://10.10.64.20";

using var plcConnection = PlcDriverManager.Default.ConnectionManager.GetConnection(connectionString);

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

var cf = writeRequest.Execute<PlcWriteResponse>();
var response = cf.Get();

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

```

## Subscribe to PLC

The following code connects to a Siemens PLC using the _s7_ protocol and subscribe to data change of some tags:

``` C#

ConcurrentDictionary<int, PlcConsumerRegistration> _registrationMap = new();
Consumer<PlcSubscriptionEvent> _plcEvent = null;
const string connectionString = "s7://10.10.64.20";

using var plcConnection = PlcDriverManager.Default.ConnectionManager.GetConnection(connectionString);

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

var cf = subscriptionRequest.Execute<PlcSubscriptionResponse>();
var response = cf.Get();

_plcEvent ??= new Consumer<PlcSubscriptionEvent>()
{
    OnAccept = (e) =>
    {
        foreach (Java.Lang.String tagName in e.TagNames)
        {
            Console.WriteLine(e.GetPlcValue(tagName));
        }
    }
};

foreach (PlcSubscriptionHandle subscriptionHandle in response.SubscriptionHandles)
{
    var reg = subscriptionHandle.Register(_plcEvent);
    _registrationMap.GetOrAdd(reg.ConsumerId, (_) => reg); 
}

// wait many events then cleanup

foreach (var item in _registrationMap)
{
    item.Value.Unregister();
}

```