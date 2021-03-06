# VL.Stride

[Stride 3d Engine](http://stride3d.net) for VL.

Try it with vvvv, the visual live-programming environment for .NET  
Download: http://visualprogramming.net

## Contributing

- Install [git lfs](https://git-lfs.github.com/) if you don't have it already
- Checkout this repository into a folder which doesn't have spaces
- Run `git lfs install` and `git lfs pull` in the git bash (could be that this step is not needed)
- Install a vvvv gamma version from teamcity.vvvv.org as specified by the `VLVersion` property in `packages\Directory.Build.props`
- Open the solution `packages\VL.Stride.sln`, switch it to `Release` mode, set VL.Stride as startup project and press `Ctrl+F5` to start vvvv. (VL.Stride is configured as a source package automatically)

Compiling and running in `Debug` requires the [graphic diagnostic tools](https://docs.microsoft.com/en-us/windows/uwp/gaming/use-the-directx-runtime-and-visual-studio-graphics-diagnostic-features) to be installed.

## Credits

A deep bow before those who believed in VL.Stride from the beginning and substantially supported its development:

* [Marshmellow Laser Feast](http://marshmallowlaserfeast.com)
* [schnellebuntebilder](http://schnellebuntebilder.de)
* [m box](http://m-box.de)
* [Refik Anadol](http://refikanadol.com)
* Jarrad Hope
