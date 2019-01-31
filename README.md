# FindLongFilePaths
This tiny console application finds any path that is longer than a specific length and writes the path and its length to the console and debug outputs.

## Usage

`FindLongFilePaths.exe \[<file-path>\] \[<max-file-path-length>\]`

## Configuration Options

* Starting File Path (default: current directory)
  * Can be passed-in on the command line (first argument)
* `MaxFilePathLength` (default: 200)
  * Can be passed-in on the command line (second argument) or provided in the `app.config` file