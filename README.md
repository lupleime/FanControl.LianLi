# FanControl.LianLi

An unofficial LianLi plugin for [FanControl](https://github.com/Rem0o/FanControl.Releases).

[![Support](https://img.shields.io/badge/Support-Buy_Me_A_Coffee-yellow?style=for-the-badge&logo=buy%20me%20a%20coffee&color=FFDD00)](https://www.buymeacoffee.com/CameronHalter)

## Device Support

| Device                          | PID          | Status                          | Read Fan RPM | Set Fan RPM |
| ------------------------------- | ------------ | ------------------------------- | ------------ | ----------- |
| LianLi-UNI SL                   | `7750, a100` | Supported                       | ✔️           | ✔️         |
| LianLi-UNI AL                   | `a101`       | Supported                       | ✔️           | ✔️         |
| LianLi-UNI SL-Infinity          | `a102`       | Supported                       | ✔️           | ✔️         |
| LianLi-UNI SL v2                | `a103, a105` | Supported                       | ✔️           | ✔️         |
| LianLi-UNI AL v2                | `a104`       | Supported                       | ✔️           | ✔️         |

## Installation

1. Download latest [release](https://github.com/EightB1ts/FanControl.LianLi/releases). You will have 2 options once you unzip the file:
    - **FanControl.LianLiPlugin.ARGB.dll (recommended)**: This will set LED effects to match the motherboard ARGB header.
    - **FanControl.LianLiPlugin.dll**: This will not configure any LED settings.

    Please review the [Known Issues](https://github.com/EightB1ts/FanControl.LianLi#known-issues) section to determine which version is right for you.

2. Once you determine which version of the plugin you want to use, drag it into FanControl's `Plugins` folder.

## Known Issues

1. Sharing the controllers across multiple pieces of software at the same time will lead to issues. For example, using this plugin along with OpenRGB. If you want to dynamically change the RGB, please use **FanControl.LianLiPlugin.ARGB.dll** and connect the controller to your motherboard's ARGB header.

## Original UNI FAN SL minimum RPM

The original percentage conversion maps 0% to raw `42`. USB captures from
L-Connect 3 show that an original UNI FAN SL controller with PID `a100`
instead uses raw `10` as its minimum-running command on every channel:

```text
E0 20 00 0A
E0 21 00 0A
E0 22 00 0A
E0 23 00 0A
```

On the tested hub, this produced approximately 495 RPM with SL140 fans and
780-810 RPM with SL120 fans. This plugin therefore maps 0% to raw `10` only
for PID `a100`. Values above 0% and all other supported controller PIDs keep
their existing mappings. Raw zero is never sent, and 100% remains full speed.

Mapping tests can be built and run with:

```powershell
msbuild src\FanControl.LianLiPlugin.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU"
tests\FanControl.LianLiPlugin.Tests\bin\Release\FanControl.LianLiPlugin.Tests.exe
```

## Submitting An Issue

When submitting an issue, please include the Name, VID, and PID of your controller. It can be located within Device Manager:

![Device Manager](https://raw.githubusercontent.com/EightB1ts/FanControl.LianLi/main/images/DeviceManager.PNG)

## Screenshots

![Screenshot 1](https://raw.githubusercontent.com/EightB1ts/FanControl.LianLi/main/images/Screenshot1.PNG)


