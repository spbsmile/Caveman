F# kit by noobtuts.com

Before you start:
  Select Window->F# Kit, download & install any components that might be missing
  by simply clicking the download button and then installing it from your
  downloads folder.

Usage Guide:
1. Right click in the Project Area, select Create->F# Script.
2. Open and modify it.
3. Wait for F# kit to rebuild.
4. Select your GameObject, click Add Component->Scripts to add it.

Notes:
- You can not drag .fs files onto a GameObject, but you can drag components from
  the F#Out folder onto a GameObject. There will be one component for each
  compiled .fs Script file.
- F# kit looks in the default install directory for Xamarin etc.
  If you installed in another location, you can either reinstall in the
  default location or modify the F# kit Scripts accordingly.
- Do not move the F#kit folder into another folder.

How F# kit works:
- It detects .fs file reimports with an Editor Extension
- It then creates a complete Xamarin F# Library solution in the F# kit folder
- It then builds the solution and removes the temporary folder
- Results can then be found in the F#Out folder

Future:
- Unity talked about using Xamarin some day. Then F# kit wouldn't have to
  download it manually, which will make everything simpler.
- Your F# scripts are only aware of UnityEngine.dll. They can't refer to other
  C# scripts and such.