# Mineimator to Blockbench Frame Converter
Converts .miframes (from a Mineimator animation) to .animation.json (for Blockblench Animated Java).

This is a recreation of [Sarr-io's MTB](https://github.com/sarr-io/mineimator-to-blockbench) but with interpolation compatibility.

## Download
[Go to releases](https://github.com/IAmMAddieAtYou/mineimator-to-blockbench/releases)

### Features:
- Bulk conversion (just drag and drop as many files as you'd like into the window)
- Quick and easy
- Allows up to 33 different types of interpolation.
- Made by IAmMaddieAtYou

### Limitations:
I do not use either of these programs day to day. I developed this script for a film project due to my animator being more comfortable with mineimator.

This is to say that the following known limitations are listed below:
- Affected bones / ignored bones (from the Animated Java plugin in Blockbench) are not supported (most plugins that add something to the frames json in Blockbench will not work[^1])
- Bend is not supported (Blockbench does not have bend)
- You might have to rename the bones again in Blockbench because Mineimator's renaming is kind of broken (it doesn't correctly apply to the .miframes output)
