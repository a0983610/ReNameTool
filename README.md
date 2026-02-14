# ReNameTool

一個基於 C# 開發的簡單工具，用於自動掃描圖檔、計算雜湊值去重，並以序號重新命名。
A simple C# tool for scanning images, deduplicating via SHA1 hash, and renaming them sequentially.

## 繁體中文版

### 功能特點
- **自動去重**：使用 SHA1 雜湊算法，確保內容相同的圖片不會重複備份。
- **序號命名**：將圖片統一命名為四位數序號（例：0001.jpg, 0002.png）。
- **斷點續傳**：自動記錄已處理的檔案與最後編號，支援多次執行不衝突。
- **支援格式**：JPG, JPEG, PNG, GIF, SVG。

### 使用方式
1. 將 `ReNameTool.exe` 放置於您想要整理的圖片資料夾中。
2. 執行程式。
3. 程式會自動在該目錄建立 `Result` 資料夾，並將重新命名後的檔案存放於此。
4. `Result/ReNameRecordedData` 會記錄已處理的資訊，請勿隨意刪除。

---

## English Version

### Features
- **Deduplication**: Uses SHA1 hashing to ensure images with identical content are not processed twice.
- **Sequential Renaming**: Renames images into 4-digit sequences (e.g., 0001.jpg, 0002.png).
- **Persistent Progress**: Automatically records processed file hashes and the last used index in a local data file.
- **Supported Formats**: JPG, JPEG, PNG, GIF, SVG.

### Usage
1. Place `ReNameTool.exe` in the directory containing the images you want to organize.
2. Run the application.
3. The tool will create a `Result` folder and copy renamed files into it.
4. Do not delete `Result/ReNameRecordedData` if you wish to maintain the naming sequence and deduplication history.

## 開發環境 (Environment)
- **Framework**: .NET 6.0
- **OS**: Windows (x64)
