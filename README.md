# LOAFont
A small tool that automatically replaces **Lost Ark’s** `font.lpk` with a custom font.

It uses **Steam Launch Options** to ensure the custom font is applied every time the game launches.

## 🛠 How It Works
- Steam launches `LOAFont.exe` instead of the game executable.
- The tool:
    - Locates Lost Ark's installation directory using Steam's `libraryfolders.vdf`.
    - Compares the game's `font.lpk` with the custom font by checking their SHA-256 hashes.
    - Creates a `.bak` backup of the original font.
    - Replaces the original font file with the custom one.
    - Launches the game using Steam’s `%command%` arguments.

## 🚀 Installation
1. Download the latest release.
2. Place your custom `font.lpk` in the same folder as `LOAFont.exe`.
3. Open Steam and go to Library.
4. Right-click **Lost Ark → Properties → General**
5. Set the **Launch Options** to:
```
"C:\Path\To\LOAFont.exe" %command%
```
6. Launch the game normally through Steam.

## 📁 Folder Structure
```
LOAFont/
├── font.lpk (custom)
└── LOAFont.exe
```

## ⚠️ Disclaimer

This project is not affiliated with or endorsed by Amazon Games or Smilegate.  
Use at your own risk. Always keep backups of original game files.