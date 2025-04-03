---
title: PLCOnNet CLI tool of .NET suite for PLC4X™
_description: Describes the CLI tool to use PLC4X™ classes from any command-line shell
---

# PLCOnNet: CLI

## Installation

- **dotnet tool** hosted on [NuGet](https://www.nuget.org/packages/MASES.PLCOnNetCLI): check https://www.nuget.org/packages/MASES.PLCOnNetCLI/ and https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools for deep installation instructions.
- **Docker image** hosted on [GitHub](https://github.com/masesgroup/PLCOnNet/pkgs/container/plconnet) and [Docker Hub](https://hub.docker.com/repository/docker/masesgroup/plconnet/general): follow instruction within the page and general instruction on https://docs.docker.com
  * The image hosts both .NET 8 and JRE 11 runtimes

> [!IMPORTANT]
> The **dotnet tool** needs a JRE/JDK installed within the system (see [JVM identification](#jvm-identification))

## Usage

To use the CLI interface (PLCOnNetCLI) runs a command like the following:

- **dotnet tool**

```sh
plconnet -i
```

> [!IMPORTANT]
> If the previous command raises the error described in [Intel CET and PLCOnNet](usage.md#intel-cet-and-plconnet), the only solution is to apply the following workaround (within an **elevated shell**) and disable CET:
> ```sh
> 	reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\plconnet.exe" /v MitigationOptions /t REG_BINARY /d "0000000000000000000000000000002000" /f
> ```

- **Docker image**

```sh
docker run ghcr.io/masesgroup/plconnet -i
```

```sh
docker run masesgroup/plconnet -i
```

## Command switch available

_plconnet_ accepts the following command-line switch:

* **ClassToRun**: has precedence to all others and needs a second parameter which identify the command class to be executed
* **Interactive** (**i**): Activates an interactive shell
* **Script** (**s**): Executes the c# script in the file arument
* **JarList** (**jl**): A CSV list of JAR to be used or folders containing the JARs
* **NamespaceList** (**nl**): A CSV list of namespace to be used for interactive shell, PLCOnNet namespace are added automatically
* **ImportList** (**il**): A CSV list of import to be used

Plus other switches available at [Command line switch](commandlineswitch.md) page.

### JVM identification

One of the most important command-line switch is **JVMPath** and it is available in [JCOBridge switches](https://www.jcobridge.com/net-examples/command-line-options/): it can be used to set-up the location of the JVM library (jvm.dll/libjvm.so) if JCOBridge is not able to identify a suitable JRE installation.
