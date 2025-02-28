## Generated classes

The command used to build the classes is the following:

1. Download the latest version of reflection utility:

```cmd
dotnet tool update -g MASES.JNetReflector
```

2. Run the reflection utility:

```cmd
jnetreflector -TraceLevel 0 -OriginRootPath .\jars -DestinationRootPath .\src\ -ConfigurationFile .\src\configuration.json
```

The configuration is:

```json
{
  "RelativeDestinationCSharpClassPath": "net\\PLC4Net\\Generated",
  "RelativeDestinationJavaListenerPath": "jvm\\plc4net\\src\\main\\java",
  "JavaListenerBasePackage": "org.mases.plc4net.generated",
  "PreferMethodWithSignature": true,
  "OnlyPropertiesForGetterSetter": true,
  "DisableInterfaceMethodGeneration": true,
  "CreateInterfaceInheritance": true,
  "JarList": [
    "plc4j-api-0.12.0.jar"
  ],
  "OriginJavadocJARVersionAndUrls": [
    {
      "Version": 8,
      "Url": "https://www.javadoc.io/doc/org.apache.plc4x/plc4j-api/0.12.0/"
    }
  ],
  "NamespacesToAvoid": [
    "org.apache.commons.logging"
  ],
  "ClassesToBeListener": [

  ],
  "ClassesToAvoid": [

  ],
  "ClassesManuallyDeveloped": [

  ],
  "NamespacesInConflict": [
    "java.lang.module",
    "java.awt.color",
    "java.awt.desktop",
    "java.awt.image",
    "java.awt.event",
    "java.awt.font",
    "org.apache.plc4x.java"
  ]
}
```