# MySecondLife Modpack Loader
[![GitHub issues](https://img.shields.io/github/issues/nkleinert/MySecondLife-Modpack-Loader.svg?style=flat-square)](https://github.com/nkleinert/MySecondLife-Modpack-Loader/issues) [![GitHub license](https://img.shields.io/github/license/nkleinert/MySecondLife-Modpack-Loader.svg?style=flat-square)](https://github.com/nkleinert/MySecondLife-Modpack-Loader/blob/master/LICENSE)
# Information
The MySecondLife Modpack Loader source code. Because the project was finished for reasons of time, the source code is now public.  
But what does the program do? It downloads the OpenIV.asi and checks if the modifications of a server are up to date (check if the hash values match) with the client hashes. If not, the files are downloaded directly to the correct folder.

# Server config files
#### modlauncher.hashes.json
```json
{
  "hashes": [
    "90AEA983EDFA6ED9C603D7E04DFEA6CA"
  ]
}
```
#### hashes.json
```json
{
  "Update": {
    "Hash": "6E4D1665CFC60A5C538706E1B90BEDFC20193B10",
    "Size": "838,60 MB"
  },
  "Dlc": {
    "Hash": "95754AE251B8C15F2A32629C1A9ED9B950E8EBAB",
    "Size": "19,73 MB"
  },
  "OpenIV": {
    "Hash": "C7B0A2F8AD03232E4BB1433828EA18E86EC25985",
    "Size": "0,13 MB"
  }
}
```
#### version.json
```json
{"version":"0.0.3.9", "changelog":"• Bei jedem Start wird geprüft ob die Datei verändert wurde"}
```