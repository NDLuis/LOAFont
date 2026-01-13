# LOAFont
A small tool that automatically replaces **Lost Ark’s `font.lpk`** with a custom font before the game launches.

It is intended to be used through **Steam Launch Options**, ensuring the font is always applied without manual file copying.

## 🛠 How It Works
- Steam launches `LOAFont.exe` instead of the game executable
- The tool:
    - Locates Lost Ark's installation directory using Steam's `libraryfolders.vdf`
    - Compares the game's `font.lpk` with the custom font *(SHA-256)*
    - Creates a `.bak` backup of the original font
    - Replaces the original font file
    - Launches the game using Steam’s `%command%` arguments

## 🚀 Installation
1. Download the latest release
2. Place your custom `font.lpk` in the same folder as `LOAFont.exe`
3. Open Steam
4. Right-click **Lost Ark → Properties → General**
5. Set the Launch Options to:
```
"C:\Path\To\LOAFont.exe" %command%
```
6. Launch the game normally through Steam

## 📁 Folder Structure
```
LOAFont/
├── LOAFont.exe
└── font.lpk (custom)
```

## ⚠️ Disclaimer

This project is not affiliated with or endorsed by Amazon Games or Smilegate.  
Use at your own risk. Always keep backups of original game files.