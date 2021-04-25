# Evands.Pellucid - Crestron

## Release Notes

### 24/4/2021 - v1.1.0

#### Features

* Feature (#5) Add the ability to output a table to the console.
* Feature (#7) Add support for default commands without verbs.
* Feature (#9) Add alias support for commands and verbs.
* Feature (#10) Make the program slot header parameter customizable and optional.
* Feature (#24) Added the ability to customize the file the options are saved to, as well as whether the options are auto-saved.

#### Fixes

* Fix (#14) Text written to debug with braces doesn't print.
* Fix (#18) Console commands with hyphens in the values of operands are incorrectly parsed.
* Fix (#20) WriteDebugLine doesn't properly add a debugging header.
* Fix (#21) Debug methods `ForceWrite` and `ForceWriteLine` are public.
* Fix (#24) The ProgLoad command blows away the TOML options file.
* Fix (#26) The `Dump()` method does not use CRLF on 4 Series Processors.

#### Other Changes

* As part of #26 the `Dump()` method has been revised and formatting improved.
* Various console commands were updated to use Tables.
* Additional example console commands were added.
* Example console commands were updated to use new features.

### 12/4/2021 - v1.1.0-Beta.1

#### Features

* (#5) Add the ability to output a table to the console.

#### Fixes

* No fixes.

### 24/3/2021 - v1.0.1

#### Features

* No New Features

#### Fixes

* Fix (#6) Global and Command level --help flags do not work.

#### Changes

* The `ProConsole` class now registers the `LoggerCommands` class as well, for the easiest initialization of Pro commands.

### 21/3/2021 - v1.0.0

#### Fixes

* Initial Release
