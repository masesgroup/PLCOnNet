# PLC4Net: [PLC4X](https://plc4x.apache.org) suite for .NET

PLC4Net is a comprehensive suite of libraries and tools to use [PLC4X](https://plc4x.apache.org) and .NET side-by-side.

### Libraries and Tools

|PLC4Net | PLC4Net.Templates | PLC4NetCLI | PLC4NetPS |
|:---:	|:---:	|:---:	|:---:	|
|[![PLC4Net nuget](https://img.shields.io/nuget/v/MASES.PLC4Net)](https://www.nuget.org/packages/MASES.PLC4Net)<br/>[![downloads](https://img.shields.io/nuget/dt/MASES.PLC4Net)](https://www.nuget.org/packages/MASES.PLC4Net) | [![PLC4Net.Templates nuget](https://img.shields.io/nuget/v/MASES.PLC4Net.Templates)](https://www.nuget.org/packages/MASES.PLC4Net.Templates)<br/>[![downloads](https://img.shields.io/nuget/dt/MASES.PLC4Net.Templates)](https://www.nuget.org/packages/MASES.PLC4Net.Templates)| [![PLC4NetCLI nuget](https://img.shields.io/nuget/v/MASES.PLC4NetCLI)](https://www.nuget.org/packages/MASES.PLC4NetCLI)<br/>[![downloads](https://img.shields.io/nuget/dt/MASES.PLC4NetCLI)](https://www.nuget.org/packages/MASES.PLC4NetCLI)|[![PLC4NetPS](https://img.shields.io/powershellgallery/v/MASES.PLC4NetPS.svg?style=flat-square&label=MASES.PLC4NetPS)](https://www.powershellgallery.com/packages/MASES.PLC4NetPS/)|

### Pipelines

[![CI_BUILD](https://github.com/masesgroup/PLC4Net/actions/workflows/build.yaml/badge.svg)](https://github.com/masesgroup/PLC4Net/actions/workflows/build.yaml) 
[![CodeQL](https://github.com/masesgroup/PLC4Net/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/masesgroup/PLC4Net/actions/workflows/codeql-analysis.yml)
[![CI_RELEASE](https://github.com/masesgroup/PLC4Net/actions/workflows/release.yaml/badge.svg)](https://github.com/masesgroup/PLC4Net/actions/workflows/release.yaml) 

### Project disclaimer

PLC4Net is a suite, curated by MASES Group, can be supported by the open-source community.

Its primary scope is to support other, public or internal, MASES Group projects: open-source community and commercial entities can use it for their needs and support this project, moreover there are dedicated community and commercial subscription plans.

The repository code and releases may contain bugs, the release cycle depends from critical discovered issues and/or enhancement requested from this or other projects.

Looking for the help of experts? MASES Group can help you design, build, deploy, and manage applications based PLC.

---

## Scope of the project

This project aims to create a set of libraries and tools to direct access, from .NET, all the features available in the [PLC4X](https://plc4x.apache.org) since, as stated in [PLC4X GitHub repository](https://github.com/apache/plc4x), the support for C# was abandoned.
And, still in [PLC4X protocols page](https://plc4x.apache.org/plc4x/latest/users/protocols/index.html), the main supported languages are Java and Go.
This project mutuated the name of the PLC4X support for .NET and implements almost all Java classes in .NET giving to a developer the same experience programming with Java. 
The following snippets demonstrate the comparison between the [Java code](https://plc4x.apache.org/plc4x/latest/users/getting-started/plc4j.html) and the C# counterpart offered from this project.

1. Initialization

<table>
<tr>
<th>Java</th>
<th>C#</th>
</tr>
<tr>
<td>
  
```java

String connectionString = "s7://10.10.64.20";

try (PlcConnection plcConnection = PlcDriverManager.getDefault()
                                                   .getConnectionManager()
                                                   .getConnection(connectionString))
{
  ... do something with the connection here ...
}
```
  
</td>
<td>

```C#

const string connectionString = "s7://10.10.64.20";

using var plcConnection = PlcDriverManager.Default
                                          .ConnectionManager
                                          .GetConnection(connectionString);
```

</td>
</tr>
</table>

2. Check supported feature

<table>
<tr>
<th>Java</th>
<th>C#</th>
</tr>
<tr>
<td>
  
```java

// Check if this connection support reading of data.
if (!plcConnection.getMetadata().isReadSupported()) {
  logger.error("This connection doesn't support reading.");
  return;
}

```
  
</td>
<td>

```C#

if (!plcConnection.Metadata.IsReadSupported())
{
    Console.WriteLine("This connection doesn't support reading.");
    return;
}

```

</td>
</tr>
</table>

3. Prepare request

<table>
<tr>
<th>Java</th>
<th>C#</th>
</tr>
<tr>
<td>
  
```java

// Create a new read request:
// - Give the single item requested an alias name
PlcReadRequest.Builder builder = plcConnection.readRequestBuilder();
builder.addTagAddress("value-1", "%Q0.4:BOOL");
builder.addTagAddress("value-2", "%Q0:BYTE");
builder.addTagAddress("value-3", "%I0.2:BOOL");
builder.addTagAddress("value-4", "%DB.DB1.4:INT");
PlcReadRequest readRequest = builder.build();

PlcReadResponse response = readRequest.execute()
                                      .get(5000, TimeUnit.MILLISECONDS);

```
  
</td>
<td>

```C#

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


```

</td>
</tr>
</table>

4. Read information

<table>
<tr>
<th>Java</th>
<th>C#</th>
</tr>
<tr>
<td>
  
```java

for (String tagName : response.getTagNames()) {
    if(response.getResponseCode(tagName) == PlcResponseCode.OK) {
        int numValues = response.getNumberOfValues(tagName);
        // If it's just one element, output just one single line.
        if(numValues == 1) {
            logger.info("Value[" + tagName + "]: " 
                        + response.getObject(tagName));
        }
        // If it's more than one element, output each in a single row.
        else {
            logger.info("Value[" + tagName + "]:");
            for(int i = 0; i < numValues; i++) {
                logger.info(" - " + response.getObject(tagName, i));
            }
        }
    }
    // Something went wrong, to output an error message instead.
    else {
        logger.error("Error[" + tagName + "]: " 
                     + response.getResponseCode(tagName).name());
    }
}

```
  
</td>
<td>

```C#

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

</td>
</tr>
</table>


See [PLC4Net usage](src/documentation/articles/usage.md) for other examples.

### Community and Contribution

Do you like the project? 
- Request your free [community subscription](https://www.jcobridge.com/pricing-25/).

Do you want to help us?
- put a :star: on this project
- open [issues](https://github.com/masesgroup/PLC4Net/issues) to request features or report bugs :bug:
- improves the project with Pull Requests

This project adheres to the Contributor [Covenant code of conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to coc_reporting@masesgroup.com.

## Summary

* [Roadmap](src/documentation/articles/roadmap.md)
* [Current state](src/documentation/articles/currentstate.md)
* [Usage](src/documentation/articles/usage.md)
* [Performance tips](https://jnet.masesgroup.com/articles/performancetips.html)
* [APIs extensibility](https://jnet.masesgroup.com/articles/API_extensibility.html)
* [JVM callbacks](https://jnet.masesgroup.com/articles/jvm_callbacks.html)
* [PLC4Net CLI usage](src/documentation/articles/usageCLI.md)
* [PLC4Net Docker usage](src/documentation/articles/docker.md)
* [PLC4Net PowerShell usage](src/documentation/articles/usagePS.md)
* [PLC4Net Command-line switches](src/documentation/articles/commandlineswitch.md)

### News

* V0.12.0+: First version based on [PLC4X](https://plc4x.apache.org) 0.12.0

---

## Runtime engine

PLC4Net uses [JNet](https://github.com/masesgroup/JNet), and indeed [JCOBridge](https://www.jcobridge.com/) with its [features](https://www.jcobridge.com/features/), to obtain many benefits:
* **Cyber-security**: 
  * [JVM](https://en.wikipedia.org/wiki/Java_virtual_machine) and [CLR, or CoreCLR,](https://en.wikipedia.org/wiki/Common_Language_Runtime) runs in the same process, but are insulated from each other;
  * JCOBridge does not make any code injection into JVM;
  * JCOBridge does not use any other communication mechanism than JNI;
  * .NET (CLR) inherently inherits the cyber-security levels of running JVM; 
* **Direct access the JVM from any .NET application**: 
  * Any Java/Scala/Kotlin/... class can be directly managed;
  * No need to learn new APIs: we try to expose the same APIs in C# style;
  * No extra validation cycle on protocol and functionality: bug fix, improvements, new features are immediately available;
  * Documentation is shared;
* **Dynamic code**: it helps to write a Java/Scala/Kotlin/etc seamless language code directly inside a standard .NET application written in C#/VB.NET: look at this [simple example](https://www.jcobridge.com/net-examples/dotnet-examples/) and [APIs extensibility](https://jnet.masesgroup.com/articles/API_extensibility.html).

### JCOBridge resources

Have a look at the following JCOBridge resources:
- [Release notes](https://www.jcobridge.com/release-notes/)
- [Community Edition](https://www.jcobridge.com/pricing-25/)
- [Commercial Edition](https://www.jcobridge.com/pricing-25/)
- Latest release: [![JCOBridge nuget](https://img.shields.io/nuget/v/MASES.JCOBridge)](https://www.nuget.org/packages/MASES.JCOBridge)
