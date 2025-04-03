---
title: Current state of .NET suite for PLC4X™
_description: Describes the current development state of .NET suite for PLC4X™
---

# PLCOnNet development state

This release comes with some ready made classes:

* PLCOnNet:
  * Reflected almost all classes of [PLC4J](https://plc4x.apache.org/plc4x/latest/users/getting-started/plc4j.html) with the limits imposed from JNetReflector
  * Manually made some classes, or extended some of reflected one, due to limitations of JNetReflector
  * If something is not available use [API extensibility](API_extensibility.md) to cover missing classes.
* PLCOnNetCLI: added REPL shell, run Main-Class and execute C# scripts
* PLCOnNetPS: some PowerShell cmdlets