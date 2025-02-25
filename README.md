# Block Factory _(temp)_

A voxel factory game. More details as progress continues.

## Known issues:
* In some scenarios lines appear between blocks. [This is a known issue with Godot and out of my control.](https://github.com/godotengine/godot/issues/79081)
MSAA makes the lines extremely obvious

# Building

This project REQUIRES a custom build of Godot.
The way Godot handles .NET integration for modules means that you have to use a custom build for the C# bindings.

Build dependencies:
* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* [SCons](https://scons.org/pages/download.html)

1. Clone the latest version of Godot from their [Github](https://github.com/godotengine/godot):

    `git clone https://github.com/godotengine/godot.git`
2. Clone the latest version of [Godot Voxel](https://github.com/Zylann/godot_voxel) into the modules folder of the godot source with the name voxel:

    `git clone https://github.com/Zylann/godot_voxel.git modules/voxel`
3. Follow the instructions to [build Godot](https://docs.godotengine.org/en/latest/contributing/development/compiling/) with the [.NET module](https://docs.godotengine.org/en/latest/contributing/development/compiling/compiling_with_dotnet.html).