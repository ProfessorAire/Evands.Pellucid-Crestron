# Evands.Pellucid-Crestron

## Overview

This library is intended to make writing information to the console or error log more functional and feature-filled in Crestron libraries and programs.

By providing `IConsoleWriter` and `ILogWriter` interfaces you have the option and ability to easily add new writers to the `ConsoleBase` and `Logger` classes to write to alternate locations. This makes it easier to do something like write a custom TCP console agent for a VC-4 instance. The right implementation can easily be swapped in based on the runtime environment. Pre-built implementations for the `CrestronConsole` and `ErrorLog` classes are automatically added when a call to the `ConsoleBase` or `Logger` classes occur without any registered implementations.

Using the `Evands.Pellucid.Pro` library will also provide classes to make writing console commands far easier. With these classes you can add a single global command to the Crestron console and then register additional commands with that global command as you need. In addition, these commands are created by decorating classes with attributes, which allows automatic parsing and command help formatting, in addition to standardizing all your commands in a familiar manner.

## Installation

There are two different ways you can install these libraries.

### Nuget

If you're using Visual Studio 2017/19 to produce code for Series-4 processors you can install the libraries via Nuget, as there are 2 Nuget packages available, `Evands.Pellucid` and `Evands.Pellucid.Pro`. If you're using Visual Studio 2008 you can still install via the Nuget packages manually. When you add references to the `dll`s, however, the Crestron Simpl# plugin might prevent the `Evands.Pellucid.Pro` from being properly added. If this happens, it can be overcome by manually adding the references to your project file. The easiest process is to add the `Evands.Pellucid.dll` reference and then manually edit your project file, copying the `Evands.Pellucid.dll` reference and updating it to point to the `Evands.Pellucid.Pro.dll` file.

### Release Binaries

The [Releases](https://github.com/ProfessorAire/Evands.Pellucid-Crestron/releases) page has binaries for each release. The releases also contain the compiled demo project `Evands.Pellucid.ProDemo.cpz` that you can load on a processor. View the demo code [here](https://github.com/ProfessorAire/Evands.Pellucid-Crestron/tree/main/src/Evands.Pellucid.ProDemo).

### Dependencies

***Regardless of how you install this, the library functionality requires the inclusion of the*** `SimplSharpReflectionInterface.dll` ***from Crestron.***

## Basic Functionality

### Options

There are a variety of options available for the `ConsoleBase`, `Debug`, and `Logger` classes, all located in the `Evands.Pellucid.Options` class. Starting with `v1.1.0` these are no longer saved in the application directory. They are optionally saved and loaded from the file `pellucid.console-options#.toml` (where `#` == the application directory) in the `\USER\Pellucid` directory. In this manner your options are persisted across reboots. If you manually set any of these options in your code your selections will take effect after the program reboots, overriding whatever values were loaded from the configuration file.

This also means that it's possible to provide a default configuration file with your program. Using your preferred method of configuration transformation you can create a different version for `Debug` and `Release` configurations and load them automatically with the code to the processor.

The available options are:
| Name                    | Type           | DefaultValue      | Description                                                                                                                                     |
| ----------------------- | -------------- | ----------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| `ColorizeConsoleOutput` | `bool`         | `true`            | Indicates whether console output should be colorized.                                                                                           |
| `UseTimestamps`         | `bool`         | `true`            | Indicates whether debug messages should have timestamps prepended to them.                                                                      |
| `Use24HourTime`         | `bool`         | `true`            | Indicates whether debug timestamps should be formatted as `13:00:15` or `1:00:15 PM`                                                            |
| `LogLevels`             | `LogLevels`    | `LogLevels.None`  | Flagged enumeration indicating what types of messages should be logged.                                                                         |
| `DebugLevels`           | `DebugLevels`  | `DebugLevels.All` | Flagged enumeration indicating what debug messages should be printed.                                                                           |
| `Suppressed`            | `List<string>` | Empty list        | List of strings matching suppressed debug source headers as described below in [`Evands.Pellucid.Diagnostics.Debug`](#evspelluciddiagnosticsdebug). Typically these values should be added/removed via the `Debug` class methods, instead of the `Options` class. |
| `Allowed`               | `List<string>` | Empty list        | List of strings matching allowed debug source headers as described below in [`Evands.Pellucid.Diagnostics.Debug`](#evspelluciddiagnosticsdebug). Typically these values should be added/removed via the `Logger` class methods, instead of directly through the `Options` class.    |
|`AutoSave`|`bool`|`true`| Indicates whether or not the file will be auto-saved on program shutdown.

The default values saved to disk will look like:

```toml
logging-levels = "None"
console-colorizeOutput = "True"
debugging-useTimestamps = "True"
debugging-shortTimestamps = "True"
debugging-levels = "All"
suppressed = [  ]
allowed = [  ]
autosave = "True"
```

An alternative configured for release, might look like:

```toml
logging-levels = "AllButDebug"
console-colorizeOutput = "True"
debugging-useTimestamps = "True"
debugging-shortTimestamps = "True"
debugging-levels = [ "Progress", "Notice", "Uncategorized" ]
suppressed = [ "SomeClassName", "SomeOtherClassName" ]
allowed = [  ]
autosave = "True"
```

To change the location that the file is loaded from you have to set the `static` property `FilePath` on the `Evands.Pellucid.Options` class. This defaults to the `\USER\Pellucid\pellucid.console-options#.toml` (where `#` is the application number).

### `Evands.Pellucid.ConsoleBase` and `Evands.Pellucid.ProConsole`

The `ConsoleBase` class is a bit of a unique one, as it's an `abstract` class that has only static methods. It isn't intended to be used as a instanced class. As such, its constructor is protected and allows other classes to extend it, in order to provide additional static functionality, or simply make it easier to call.

The `ProConsole` in the `Evands.Pellucid.Pro` project is an example of this and extends the class to provide the ability to add the `Evands.Pellucid.Pro` library's `ConsoleCommands` and `DebuggingCommands` to a list of commands that you provide the names of to the `ProConsole.InitializeConsole` method. In most cases if you're writing S# Pro Library or Program you'll want to extend the `ProConsole` over the `ConsoleBase`, unless you manually add these commands yourself.

Generally it's easiest to call the console if you create your own implementation named `Console` that exists in the root of your project namespace. If it exists in the project's root namespace it will supersede the `System.Console` class, which will allow you to call `Console.[CommandName]` instead of `ConsoleBase.[CommandName]` or `ProConsole.[CommandName]`.

In addition, the console has a variety of options for printing colorized text, which makes it far easier to differentiate messages when reading a busy console. You can specify a variety of standard console colors, or use an implementation of the `Evands.Pellucid.Terminal.Formatting.IConsoleColor` interface, such as `Evands.Pellucid.Terminal.Formatting.RgbColor` to specify a custom color. These colors are only printed if the `Evands.Pellucid.Options.Instance.ColorizeConsoleOutput` property is set to `true`. (By default it is.)

Finally, messages printed to the console using the `Write` and `WriteLine` methods are prefixed with the numer of the program slot that the code is executing in, formatted like `"[01] Your message here."`, which can make it easier to differentiate between programs writing to the console.

#### Console Examples

```csharp
using Evands.Pellucid;
using Evands.Pellucid.Terminal;
using Evands.Pellucid.Terminal.Formatting;

// Registering the default CrestronConsoleWriter. (This happens automatically if none are registered.)
ConsoleBase.RegisterConsoleWriter(new CrestronConsoleWriter());

// Registering a custom console writer.
ConsoleBase.RegisterConsoleWriter(new CustomImplementationOfIConsoleWriter());

// All registration should happen a single time, somewhere in your project initialization code.
// If you aren't using any custom writers, this can be ignored as it will automatically happen.

// Basic message writing
ConsoleBase.WriteLine("This is a message with no color.");
ConsoleBase.WriteLine(new ColorFormat(ConsoleBase.Colors.Magenta, ColorCode.None), "This message text is written in Magenta.");
ConsoleBase.WriteLine(ConsoleBase.Colors.Black, ConsoleBase.Colors.BrightRed, "This message is black text on a bright red background.");
ConsoleBase.WriteLine(new RgbColor(0xFF, 0xB6, 0xF8), "This message is written in light pink.");
```

### `Evands.Pellucid.Diagnostics.Debug`

Although the `ConsoleBase` class provides a variety of methods for writing messages directly to the console, in almost all cases, it probably isn't the manner you want to print messages to the console.

The `Evands.Pellucid.Diagnostics.Debug` class provides a large variety of methods for printing messages to the console that are only printed when the `Evands.Pellucid.Options.DebugLevels` property contains the appropriate levels.

> The available debug flags are: `None`, `Debug`, `Progress`, `Success`, `Error`, `Notice`, `Warning`, `Exception`, `Uncategorized`, `All`, and `AllButDebug`.

Messages printed in this manner are also printed with a header, in the final format `[Slot#][TimeStamp][Source] Message`. The first parameter passed into all `Debug.Write[Level]` or `Debug.Write[Level]Line` methods is an object, normally the object that the method is originating from. When these methods are called the header is determined by the following steps:

1. If the object provided is `null` then the message is printed without the source header.
2. If the object provided is a `string` then the source header is the string provided.
   * This is a good way to provide a source header for static classes or static methods.
3. If the object provided implements the `IDebugData` interface then the source header text and *color* are provided by the interface's properties.
   * This is the easiest way to define a header and header color for a class.
4. If the object doesn't implement `IDebugData` then the name of the class itself is used.

When a string or the type name is used, the `Debug` class checks to see if the string has been registered with an associated `ColorFormat`, which defines foreground and background `colors.`

> Headers can be registered by calling `Debug.RegisterHeaderObject(yourClass, yourColorFormat);` or `Debug.RegisterHeaderObject("StringValue", yourColorFormat);`

Besides restricting messages by their type, you can also narrow which debug messages are printed in several ways. You can add source header text to an `Allowed` or `Disallowed` list. (`Debug.AddAllowed` or `Debug.AddDisallowed`) When there are *any* items in the `Allowed` list, then *only* items in that list are allowed to print. This is especially useful in program code, as using the provided `Evands.Pellucid.Terminal.DebuggingCommands` makes it easy to add and remove these on the fly from the console. If you're working on debugging a specific object you can add just the objects you want to allow into this list.

Likewise, if you have a particularly chatty object that prints many debug messages and you want to explicitly enable viewing these messages only when you want to, add it to the `Disallowed` list, which will prevent messages originating from objects with that source header from being printed, regardless of the `DebugLevels` property.

#### Debug Examples

```csharp
using Evands.Pellucid.Diagnostics;
using Evands.Pellucid.Terminal.Formatting;

public ExampleClass : IDebugData
{
    // Instead of printing "ExampleClass" in the source header just "Example" will be printed.
    private readonly string header = "Example";

    // Defines an orange header color.
    private readonly ColorFormat headerColors = new ColorFormat(new RgbColor(255, 143, 78));

    string IDebugData.Header { get { return header; } }

    ColorFormat IDebugData.HeaderColor { get { return headerColors; } }

    public void WriteExamples()
    {
        // This prints a debug message, which only is written when the DebugLevels.Debug flag is set in the Options.DebugLevels property.
        // It also is written using the ConsoleBase.Colors.Debug ColorFormat.
        Debug.WriteDebugLine(this, "This is a debug message.");

        // These are progress messages. Only the last one is written with a line break and are only printed when the DebugLevels.Progress flag is set.
        // Since these do not have line breaks, each consecutive message isn't prepended with a header, allowing long running operations to be
        // tracked by doing things like printing dots. This can be especially useful in console commands.
        // They're written using the ConsoleBase.Colors.Progress ColorFormat.
        Debug.WriteProgress(this, "Doing something.");
        Debug.WriteProgress(this, ".");
        Debug.WriteProgress(this, ".");
        Debug.WriteProgress(this, ".");
        Debug.WriteProgress(this, ".");
        Debug.WriteProgressLine(this, ".");

        // A notice message, written when the DebugLevels.Notice flag is set, using the ConsoleBase.Colors.Notice ColorFormat.
        Debug.WriteNoticeLine(this, "This is a notice message.");

        // A success message, written when the DebugLevels.Success flag is set, using the ConsoleBase.Colors.Success ColorFormat.
        Debug.WriteSuccessLine(this, "This is a success message.");

        // A warning messsage, written when the DebugLevels.Warning flag is set, using the ConsoleBase.Colors.Warning ColorFormat.
        Debug.WriteWarningLine(this, "This is a warning message.");

        // An error message, written when the DebugLevels.Error flag is set, using the ConsoleBase.Colors.Error ColorFormat.
        Debug.WriteErrorLine(this, "This is an error message.");

        var ex = new ArgumentNullException("Dummy");
        // Writes the exception to the console, written when the DebugLevels.Exception flag is set, using the ConsoleBase.Colors.Exception ColorFormat.
        Debug.WriteException(this, ex, "Example dummy exception.");

        // Writes a message to the console, written when the DebugLevels.Uncategorized flag is set.
        Debug.WriteLine(this, new ColorFormat(new RgbColor(111, 222, 111)), "This is an uncategorized custom message, written in the color you specify.");
    }
}
```

Theoretical output of this might look like the following:

```text
[01][11:11:11][ControlSystem] This is a debug message.
[01][11:11:11][ControlSystem] Doing something......
[01][11:11:11][ControlSystem] This is a notice message.
[01][11:11:12][ControlSystem] This is a success message.
[01][11:11:12][ControlSystem] This is a warning message.
[01][11:11:12][ControlSystem] This is an error message.
[01][11:11:12][ControlSystem] Example dummy exception.
--------Exception 1--------
System.ArgumentNullException: Value can not be null.
Parameter name: Dummy
-----------------------------


[01][11:11:13][ControlSystem] This is an uncategorized custom message, written in the color you specify.");
```

Coloring of each line is dependent on the message type.

> If the default color formats aren't to your liking, you can customize all of them by setting the corresponding property in the `ConsoleBase.Colors` class.

### `Evands.Pellucid.Diagnostics.Logger`

The `Logger` class is useful for situations where you want to log messages somewhere, so they can be examined at a later date. Typically this is done with the `ErrorLog` class, which is what the default `CrestronLogWriter` implementation provided does. In addition to logging, messages logged will *also* be printed to the console via the `Debug` class, provided the correct `DebugLevels` flag is set.

Like the `Debug` class, the `Logger` class has levels that can be enabled or disabled independently of the `Debug` levels, located in the `Evands.Pellucid.Options.LogLevels` property.

> The available log flags are: `None`, `Notice`, `Warning`, `Error`, `Exception`, `Debug`, `All`, and `AllButDebug`.

> Since the `LogLevels` and `DebugLevels` properties are independent of one another, this allows things like disabling the logging of messages to disk while writing a program and instead seeing them printed to the console. When you deploy to production you might enable all the logging to disk (usually excluding the `Debug` log level) and either disable debug writing altogether, or remove the `Notice`, `Warning`, `Error`, `Exception`, and `Debug` flags, so that only `Progress`, `Success`, and `Uncategorized` messages are printed to console.

Logged messages are first printed via the `Debug` class, provided the appropriate `DebugLevels` flag is set. Then they're logged using all registered `ILogWriter` implementations. If no writer is registered the default `CrestronLogWriter` implementation is automatically registered for use.

There are extension methods that can provide faster access to writing to the `Logger`, which extend any `object`.

#### Logger Examples

```csharp
using Evands.Pellucid.Diagnostics;

public ExampleClass : IDebugData
{
    public void DoSomething()
    {
        try
        {
            // Logs a warning to the registered ILogWriter implementations, printing it to the console if enabled as well.
            Logger.LogWarning(this, "We're about to DoSomething and it might break!");

            // Do Something

            // Logs a debug message to the registered ILogWriter implementations, printing it to the console if enabled as well.
            Logger.LogDebug(this, "We finished doing something and it worked! Whew!");
        }
        catch (Exception ex)
        {
            // Logs an exception to the registered ILogWriter implementations, printint it to the console if enabled as well.
            // You'll note in this case we used the Extension method, calling it from the 'this' object.
            this.LogException(ex, "Exception while doing something!");
        }
    }
}
```

### Console Commands

When using the `Evands.Pellucid.Pro` library you'll have access to a variety of classes that make creating console commands far easier. When using commands written this way they're far easier to explore and execute via the console, as help messages are automatically generated and formatted based on the attributes you apply to the classes and methods.

There are a few attributes that contribute to this functionality, all located in the `Evands.Pellucid.Terminal.Commands.Attributes` namespace. These are all demonstrated in the sample below.

#### Command Example

```csharp
using Evands.Pellucid;
using Evands.Pellucid.Terminal.Commands.Attributes;
using Evands.Pellucid.Terminal.Commands;

    [Command("example", "Example console command.")]
    public class ExampleCommands : TerminalCommandBase
    {
        [Verb("echo", "Echoes the message provided back to the console.")]
        [Sample("example echo --message \"Just a test.\" -c", "Prints the message \"JUST A TEST.\" to the console.")]
        public void EchoMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps)
        {
            ConsoleBase.WriteCommandResponse(caps ? message.ToUpper() : message);
        }

        [Verb("echo", "Echoes the message provided back to the console in a specified color.")]
        [Sample("example echo --message \"Just a test.\" -cr", "Prints the message \"JUST A TEST.\" to the console in red.")]
        public void EchoRedMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps,
            [Flag("red", 'r', "When present prints the message in red.")] bool red)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, caps ? message.ToUpper() : message);
        }

        [Verb("echo", "Echoes the message provided back to the console in a specified color.")]
        [Sample("example echo --message \"Just a test.\" -cb", "Prints the message \"JUST A TEST.\" to the console in blue.")]
        public void EchoBlueMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps,
            [Flag("blue", 'b', "When present prints the message in blue.")] bool blue)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightBlue, caps ? message.ToUpper() : message);
        }

        [Verb("echo-green", "Echoes the message provided back to the console in green.")]
        [Sample("example echo-green --message \"Just a test.\"", "Prints the message \"Just a test.\" to the console in green.")]
        public void EchoGreenMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightGreen, caps ? message.ToUpper() : message);
        }
    }
```

You can see how commands with the same name are permissible, and the presence of the correct operands and flags on the command line dictate which method is executed.

#### Using Commands

You have to create and register a `GlobalCommand` with the `CrestronConsole` in order to use it.

```csharp
// Inside the ControlSystem constructor method.
// Setup the global command(s).
var appCommand = new GlobalCommand("app", "Application commands.", Access.Programmer);
appCommand.AddToConsole();

// This is an example of how the ProConsole provides quick initialization of both
// the ConsoleCommands and the Debug
ProConsole.InitializeConsole("app");
```

Then you can register the custom commands you write with that `GlobalCommand` anywhere it makes sense.

```csharp
// Anywhere in your code registering a command is needed.
var exampleCommands = new ExampleCommands();
exampleCommands.RegisterCommand("app");
```

From there you use the commands via the console. The examples below assume you're executing via the standard Crestron console in an application on slot 1.

```text
RMC3>app:1 example echo --message "This has no color." -c
THIS HAS NO COLOR.

RMC3>app:1 example echo --message "This is in red." -cr
THIS IS IN RED.

RMC3>app:1 example echo --message "This text is blue!" -b
This text is blue!

RMC3>app:1 example echo-green --message "This message is green."
This message is green.
```

Looking at the output you can see that creating console commands in this manner allows for super powerful interactions with little to no effort, apart from organizing your commands logically. In our case the framework handles converting `OperandAttribute` values to `string`, `int`, `double`, `bool`, etc. Creating commands in this manner allows you to focus on the command functionality without having to parse values or do any regex matching to determine what values have been provided.

The `Evands.Pellucid.Pro` library has several classes that demonstrate powerful console commands for configuring the `Debug`, `Logger`, and `ConsoleBase` options, such as `DebugLevels`, `ColorizeConsoleOutput`, etc.

### Tables

Starting in `v1.1.0-Beta.1` the ability to create tables has been added. The classes to do so are located in `Evands.Pellucid.Terminal.Formatting.Tables`, namely the `Table` class, which is designed with methods to allow quickly building a table to print to the console using method chaining.

```csharp
ConsoleBase.WriteLine();
ConsoleBase.WriteLineNoHeader(
    Table.Create()
    .AddColumnWithHeader("Device", "Touchpanel", "DSP", "Codec", "Display 1", "Display 2")
    .AddColumnWithHeader("Status", "Online", "Online", "Offline", "Offline", "Online")
    .FormatHeaders(ConsoleBase.Colors.BrightYellow, HorizontalAlignment.Center)
    .FormatColumn(0, ConsoleBase.Colors.BrightCyan, HorizontalAlignment.Right)
    .ForEachCellInColumn(1, c => c.Color = c.Contents == "Offline" ? ConsoleBase.Colors.BrightRed : ConsoleBase.Colors.BrightGreen).ToString());
```

The above code would print a table that looks like the following, except with colors.

```text
------------------------
|   Device   | Status  | 
|----------------------|
| Touchpanel | Online  | 
|----------------------|
|        DSP | Online  | 
|----------------------|
|      Codec | Offline | 
|----------------------|
|  Display 1 | Offline | 
|----------------------|
|  Display 2 | Online  | 
------------------------
```

As demonstrated, each of the primary methods on the `Table` class return the instance of the table being interacted with, allowing the chaining of the methods. There are a variety of commands available, including `AddRow`, `AddColumn`, `WithHeaders`, `ForEachCellInColumn`, all allowing specific methods for adding data to the table.

Each of the cells in a table also have a couple specific formatting commands available, with the ability to set:

* Color - Prints all text in a cell in that color
* HorizontalAlignment - Aligns the text in a cell to the left, center, or right.

In addition, a table can be formatted with a maximum width cells are allowed to consume, which will cause text that is longer than the specified value to be wrapped. You can also set the minimum width cells must consume, which padds cells that would be shorter (based on the text contents of the column) than the provided value.
