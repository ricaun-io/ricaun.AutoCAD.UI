# ricaun.AutoCAD.UI

[![AutoCAD 2019](https://img.shields.io/badge/AutoCAD-2019+-E51050.svg)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/ricaun.AutoCAD.UI/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/ricaun.AutoCAD.UI/actions)
[![Release](https://img.shields.io/nuget/v/ricaun.AutoCAD.UI?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.AutoCAD.UI)

[![ricaun AutoCAD UI](https://github.com/user-attachments/assets/839c9094-e28d-4c0f-8750-714e8fc64599)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)

`ricaun.AutoCAD.UI` provides a set of UI controls and utilities for AutoCAD .NET applications, built on top of the `AutoCAD.NET` library. It is designed to simplify the development of ribbon user interfaces in AutoCAD plugins.

## Features

### ExtensionApplication

The `ExtensionApplication` class provides a base implementation for AutoCAD .NET applications to start and shutdown, managing the ribbon control lifecycle.

```C#
using ricaun.AutoCAD.UI;
using Autodesk.AutoCAD.Runtime;

[assembly: ExtensionApplication(MyExtensionApp)]

public class MyExtensionApp : ExtensionApplication
{
    public override void OnStartup(RibbonControl ribbonControl)
    {
        ribbonControl.CreateOrSelectPanel("MyPanel", "MyTab")
    }

    public override void OnShutdown(RibbonControl ribbonControl)
    {
        ribbonControl.RemovePanel("MyPanel", "MyTab");
    }
}
```

## Release

* [Latest release](https://github.com/ricaun-io/ricaun.AutoCAD.UI/releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/ricaun-io/ricaun.AutoCAD.UI/stargazers)!