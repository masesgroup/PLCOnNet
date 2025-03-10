---
title: Current state of PLC4X suite for .NET
_description: Describes the current development state of PLC4X suite for .NET
---

# PLC4Net development state

This release comes with some ready made classes:

* PLC4Net:
  * Reflected almost all classes of [PLC4J](https://plc4x.apache.org/plc4x/latest/users/getting-started/plc4j.html) with the limits imposed from JNetReflector
  * Manually made some classes, or extended some of reflected one, due to limitations of JNetReflector
  * If something is not available use [API extensibility](API_extensibility.md) to cover missing classes.
* PLC4NetCLI: added REPL shell, run Main-Class and execute C# scripts
* PLC4NetPS: some PowerShell cmdlets