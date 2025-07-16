# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.3.0] / 2025-07-15
### Features
- Support `CreateSplitButton` extension.
- Support `CreatePulldownButton` extension.
### UI
- Update `RibbonExtension` to support `CreateSplitButton` and `CreatePulldownButton`.
- Update `MessageBox.ShowMessage` to close window using `Key.Escape`.
- Add `SetListImageSize` to set the image size for `RibbonListButton`.
### Example
- Add `CreateSplitButton` and `CreatePulldownButton` to the example project.

## [0.2.0] / 2025-07-04
### Features
- Add `ricaun.AutoCAD.UI.Example` sample project.
### UI
- Add `AddSeparator` extension to create a separator in the ribbon panel.
- Add `InfoCenter` to display balloon information in the AutoCAD InfoCenter.
- Add `RibbonListButton` in the `RibbonThemePanelUtils`.
### Example
- Add `CircleCreate` command to the example project.
- Add `MessageBox` and `ShowBalloon` buttons to the example ribbon panel.
- Update resource image to `Cube-Grey-Light.tiff` and `Cube-Grey-Dark.tiff`.
- Add `AutoCADBusyService` to update the button state in the example project.
- Add `AutoCADTaskService` to create multiple circles in the example project. 
- Add `PaletteSetUtils` to show a palette set in the example project.
### Updates
- Fix `CheckURLValid` to support `http` and `https`.
- Fix `SetImage` to update `Image` property.
- Update `LockDocumentRelayCommand` to use `LockDocumentManager` to support document locking.
- Update `AutoCADTaskService` update `Application_Idle` to run single event handler.

## [0.1.0] / 2025-07-02
### Features
- `ExtensionApplication` and `RibbonControl` extension for AutoCAD ribbon UI.
### UI
- Create `ricaun.AutoCAD.UI` project.
- Add `ExtensionApplication` and `RibbonExtension` classes.
- Add `Busy`, `Runtime`, `Tasks`, and `Windows` utilities.

[vNext]: ../../compare/1.0.0...HEAD
[0.3.0]: ../../compare/0.2.0...0.3.0
[0.2.0]: ../../compare/0.1.0...0.2.0
[0.1.0]: ../../compare/0.1.0