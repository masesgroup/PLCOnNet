---
title: PLCOnNet PowerShell module of .NET suite for PLC4X
_description: Describes the PowerShell module to use PLC4X classes from any PowerShell shell
---

# PLCOnNet: PowerShell Module

## Installation

To install the tool executes the following command within a PowerShell shell:

```powershell
Install-Module -Name MASES.PLCOnNetPS
```

If the above command fails, reporting errors related to _authenticode_, use the following command:

```powershell
Install-Module -Name MASES.PLCOnNetPS -SkipPublisherCheck
```

## Usage

To use the PowerShell interface (PLCOnNetPS) runs the following commands within a **PowerShell** shell:

### Initialization

* The following cmdlet initialize the environment:

```powershell
Start-PLCOnNetPS [arguments]
```

### Execution

Now everything is ready and you can create objects like in the following snippet:

```powershell
$usrPwd = New-PlcUsernamePasswordAuthentication -Username mases -Password ThePassword
$connection = Get-PlcConnection -Url "s7://10.10.64.20" -Authentication $usrPwd
$connection.Connect()
$connection.IsConnected()
```

Another snippet can be:

```powershell
Get-ProtocolCodes
Get-PlcDriver -ProtocolCode "s7"
```

## Cmdlet available

_plconnetps_ accepts the following cmdlets:

* **Start-PLCOnNetPS**: Initialize the engine and shall be the first command to be invoked. The arguments are:
  * LicensePath
  * JDKHome
  * JVMPath
  * JNIVerbosity
  * JNIOutputFile
  * JmxPort
  * EnableDebug
  * JavaDebugPort
  * DebugSuspendFlag
  * JavaDebugOpts
  * HeapSize
  * InitialHeapSize
  * LogClassPath
* **New-PlcUsernamePasswordAuthentication**: returns an authentication object. The arguments are:
  * Username
  * Password
* **Get-ProtocolCodes**: returns the list of current managed protocols.
* **Get-PlcDriver**: returns a PlcDriver object from the protocol code. The argument is:
  * ProtocolCode
* **Get-PlcConnection**: returns a PlcConnection object from the connection string. The arguments are:
  * Url
  * Authentication (optional)
* **Start-OPCUAServer**: starts an OPCUA server instance. The arguments are the same expected from CLI
* **Start-Plc4xServer**: starts an PLC4X server instance. The arguments are the same expected from CLI
* 
### JVM identification

One of the most important command-line switch is **JVMPath**: it can be used to set-up the location of the JVM library (jvm.dll/libjvm.so) if JCOBridge is not able to identify a suitable JRE installation.
