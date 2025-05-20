# DiskIndex

**DiskIndex** is a personal disk cataloging tool built with Blazor Server and MudBlazor.  
It scans a selected root folder and indexes all files into a local SQLite database, displaying them through a clean and responsive web UI.

This is my first real project published on GitHub as a new C# developer. It started as a personal learning experience, but I decided to make it **open-source** to share it with others and continue improving it. The base features are fully functional, and more updates are planned. Check out the **TODO** section if you're curious!

## 💡 Why DiskIndex?

Managing large file collections can quickly become overwhelming — especially when dealing with deep folder hierarchies and chaotic file naming. **DiskIndex** was built to simplify this, offering a lightweight, centralized digital catalog system.

This project is ideal for data hoarders, collectors, or anyone trying to keep track of thousands of files without wasting time navigating endless subfolders. DiskIndex indexes all your files, keeps the database synced with the disk, and provides a web-based interface to browse, search, and manage everything effortlessly.

If your collection is meant to be accessed remotely by others — but it's a mess — a clean UI and searchable grid will help people who didn’t create the structure to quickly get oriented and find what they need.

## Features

- Categorize files using custom folder rules (`Configs/categories.json`)
- Tracks file name, size, type, last modified date, and more
- Fast dynamic search and sorting grid
- JSON-based configuration for root and category folders
- Clean UI powered by [MudBlazor](https://mudblazor.com/)

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- A local folder with files to index

### How to Run

1. **Clone** the repository  
2. **Configure** your root folder in `Configs/rootFolderPath.json`:  
    ```json
    {
      "rootFolderPath": "D:\\MyFiles"
    }
    ```
    *(This step will be optional and improved in future updates)*

3. *(Optional)* Define categories based on folders in `Configs/categories.json`:  
    ```json
    {
      "Books": "D:\\MyFiles\\Books",
      "Photos": "D:\\MyFiles\\Photos"
    }
    ```

4. **Run** the project using your IDE (e.g., Visual Studio)

## 🧠 How It Works (Technical Overview)

DiskIndex uses a Blazor Server app with the following architecture:

- `FileSyncService` scans the disk and updates a local SQLite database (`FileDatabase.db`).
- The files are stored in the `FileRecord` table with metadata like path, size, category, and NSFW flag (optional to filter files).
- A `CatalogService` handles database queries, exposed to the UI via dependency injection.
- UI is built with MudBlazor, and includes a searchable, sortable `MudDataGrid`.

All configuration (like root path and categories) is done via JSON files inside the `Configs/` folder.

The app build will create a new database file if none is found and use it to store all the records. The files that is going to scan are everything **from and after** the edited `rootFolderPath`; for example if your path in the JSON file is:
    
    "rootFolderPath": "D:\\root\\MyFiles"

The system will read files located in "MyFiles" and every other sub-folder inside of it.

In order to fully scan the folders, DiskIndex has a simple button that you can click, it will begin the process and when its done the grid below will show and list every file (in his scope). Listed files fed to the grid can be sorted and searched through a fast searchbar.

For a more detailed "code stuff" I tried to comment every important methods in the classes itself, especially useful if you want to change it as you wish. There are some unused block of codes that I kept them in case they are needed in the future like `FileServicesOLD.cs`, which was the first iteration to read the files and insert them on a List<FileItem>, this has being replaced with the present `FileSyncService` that uses a SQLite Database to store the info after a scan, while removing and adding the records if files in the folders has been modified.

## 🛠️ TODO

As I said the project will receive additional updates even if the base features works as intended. I'm open for suggestions!

- [ ] First Setup System (The webapp will ask the root path when run the first time or if the JSON file is not populated)
- [ ] NSFW Tagging System that uses a page to edit if files are supposed to be NSFW or not (useful for websites)
- [ ] Add file preview (for images/videos)
- [ ] User authentication system (optional)
- [ ] Deploy a live demo version (optional)
- [ ] Create Dockerfile and publish on DockerHub for easier testing
- [ ] Improve the interface (design wise)

## 📄 License

This project is licensed under the [MIT License](LICENSE), 
a permissive open source license that allows reuse, modification, and distribution with proper attribution.
Please note that this software is provided "as is", without warranty of any kind, express or implied.

## 📬 Contact

Created by [@svetlys](https://github.com/svetlys)  
Feel free to reach out!     📩 alessiob.develop@gmail.com
