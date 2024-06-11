# Mineimator to Blockbench Frame Converter
Converts .miframes (from a Mineimator animation) to .animation.json (for Blockblench).

<img src="https://github.com/sarr-io/mineimator-to-blockbench/assets/49985341/699e932c-2171-4745-b00f-fce9667b4021" width="400" height="295"/>

## Download
[Go to releases](https://github.com/sarr-io/mineimator-to-blockbench/releases)

### Features:
- Bulk conversion (input/output folders to drag your files into)
- Uses proper decimal floats instead of zig's scientific notation default (not really a feature)
- Very fast 💯
- Made by me :)

### Limitations:
I do not use either of these programs (in fact I dont even have them installed). I developed this script for a friend who needed to use Mineimator's animation tools for a Blockbench (more specifically Animated Java) project.

This means the script is mostly built around the features that were required for his project to work. With that being said, known limitations are listed below:
- Interpolation types (smooth, instant, etc) are not supported (Blockbench's json output changes significantly if any interpolation mode is used outside of linear, and it would require extreme changes to my code to get working)
- Affected bones / ignored bones (from the Animated Java plugin in Blockbench) are not supported (most plugins that add something to the frames json in Blockbench will not work[^1])
- Bend is not supported (Blockbench does not have bend)
- You might have to rename the bones again in Blockbench because Mineimator's renaming is kind of broken (it doesn't correctly apply to the .miframes output)
- Maximum json filesize is 1mb (hard-coded), you will never exceed this (you can increase the limit yourself if you somehow do)

## Build
```zig
zig build run
```
```zig
zig build
```
Note: add an "input" and "output" folder (exact naming) in the same directory as build.zig (if using the first command) or the resulting .exe (see the [releases](https://github.com/sarr-io/mineimator-to-blockbench/releases) for an example).

## Contribution & Issues:
I am fine with fixing some bugs and possibly adding some small compatability features, but if you want a pretty large or complicated compatability feature: fork this repo, add it yourself, and then make a pull request.

[^1]: "This means the script is mostly built around the features that were required for his project to work."
