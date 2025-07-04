# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.2.0] / 2025-07-04
### Features
- Add `ricaun.AutoCAD.UI.Example` sample project.
### UI
- Add `AddSeparator` extension to create a separator in the ribbon panel.
### Example
- Add `CircleCreate` command to the example project.
### Fixes
- Fix `CheckURLValid` to support `http` and `https`.
- Fix `SetImage` to update `Image` property.
- Update `LockDocumentRelayCommand` to use `LockDocumentManager` to support document locking.

## [0.1.0] / 2025-07-02
### Features
- `ExtensionApplication` and `RibbonControl` extension for AutoCAD ribbon UI.
### UI
- Create `ricaun.AutoCAD.UI` project.
- Add `ExtensionApplication` and `RibbonExtension` classes.
- Add `Busy`, `Runtime`, `Tasks`, and `Windows` utilities.

[vNext]: ../../compare/1.0.0...HEAD
[0.2.0]: ../../compare/0.1.0...0.2.0
[0.1.0]: ../../compare/0.1.0