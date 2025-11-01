# ricaun.AutoCAD.UI

[![AutoCAD 2019](https://img.shields.io/badge/AutoCAD-2019+-E51050.svg)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/ricaun.AutoCAD.UI/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/ricaun.AutoCAD.UI/actions)
[![Release](https://img.shields.io/nuget/v/ricaun.AutoCAD.UI?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.AutoCAD.UI)

[![ricaun AutoCAD UI](https://raw.githubusercontent.com/ricaun/test-assets/main/assets/ricaun.AutoCAD.UI.png)](https://github.com/ricaun-io/ricaun.AutoCAD.UI)

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
        ribbonControl.CreatePanel("MyPanel");
    }

    public override void OnShutdown(RibbonControl ribbonControl)
    {
        ribbonControl.RemovePanel("MyPanel");
    }
}
```

### RibbonExtension

The `RibbonExtension` class offers extension methods to create and manage ribbon panels, buttons, and other UI elements in AutoCAD.
```C#
using ricaun.AutoCAD.UI;
using Autodesk.AutoCAD.Runtime;

[assembly: ExtensionApplication(MyExtensionApp)]

public class MyExtensionApp : ExtensionApplication
{
    public override void OnStartup(RibbonControl ribbonControl)
    {
        var ribbonPanel = ribbonControl.CreatePanel("MyPanel");

        ribbonPanel.CreateButton("MyButton")
            .SetText("Click Me")
            .SetDescription("This is a button description")
            .SetToolTip("This is a button tooltip")
            .SetImage("Resources/image16-light.png")
            .SetLargeImage("Resources/image32-light.png")
            .SetCommand(() => 
            {
                // Button click logic here
            });
    }

    public override void OnShutdown(RibbonControl ribbonControl)
    {
        ribbonControl.RemovePanel("MyPanel");
    }
}
```

### Ribbon Image - Theme Change Support

The `SetImage` and `SetLargeImage` methods automatically handle theme changes based in the key name `light` and `dark` in the image file names.

```C#
ribbonPanel.CreateButton("MyButton")
    .SetImage("Resources/image16-light.png") // If the theme is dark, it will use "Resources/image16-dark.png" internally
    .SetLargeImage("Resources/image32-light.png"); // If the theme is dark, it will use "Resources/image32-dark.png" internally
```

When the AutoCAD theme changes, the ribbon button images will automatically update to match the current theme.

## Release

* [Latest release](https://github.com/ricaun-io/ricaun.AutoCAD.UI/releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/ricaun-io/ricaun.AutoCAD.UI/stargazers)!