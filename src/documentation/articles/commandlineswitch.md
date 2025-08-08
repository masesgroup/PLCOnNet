---
title: Command-line switch of .NET suite for PLC4X™
_description: Describes the command-line switch of .NET suite for PLC4X™
---

# PLCOnNet: Command line switches available

_plconnet_ accepts the following command-line switches:

* **LogClassPath**: put in command line the switch to output all Jars found
* **CommonLoggingConfiguration**: The file containing the configuration of common logging
* **LogPath**: The path where log will be stored

Plus it accepts from:
* **JCOBridge**:
  * All command-line switches of JCOBridge available at https://www.jcobridge.com/net-examples/command-line-options/

## JVM™ identification

One of the most important command-line switch is **JVMPath** and it is available in [JCOBridge switches](https://www.jcobridge.com/net-examples/command-line-options/): it can be used to set-up the location of the JVM™ library if JCOBridge is not able to identify a suitable JRE installation.
If a developer is using PLCOnNet within its own product it is possible to override the **JVMPath** property with a snippet like the following one:

```c#
    class MyPLCOnNetCore : PLCOnNetCore
    {
        public override string JVMPath
        {
            get
            {
                string pathToJVM = "Set here the path to JVM library or use your own search method";
                return pathToJVM;
            }
        }
    }
```

