<h1 align="center">OneFileEncryptDecrypt</h1>
<h3 align="center">One file encrypt decrypt in my machine.</h3>
<h3 align="center">Protect my important file.</h3>

## Require

.NET 10

## Optional

VisualStudio 2026 or Higher

## Get Started

### 1. Clone this repository.

### 2-1. VisualStudio Build

- Open solution
- Build

### 2-2. Command Line Build 

- Move clone directory

```bash
dotnet build
dotnet build -c Release
```

### 3. Move Binary Directory 

`SOLUTION_DIRECTORY` > `OneFileEncryptDecrypt` > `bin` > `Debug`

OR

`SOLUTION_DIRECTORY` > `OneFileEncryptDecrypt` > `bin` > `Release`


### 4. Install

Install is very simple.

Just copy binary directory all files want directory. 😁

### 5. Execute

```bash
OneFileEncryptDecrypt.exe
```

## Using

### CreateSalt

- Create crypto salt.

```bash
OneFileEncryptDecrypt createsalt
```

### Encrypt

- Encrypt file

```bash
OneFileEncryptDecrypt encrypt -p <PASSWORD> -f <ENCRYPT_FILE_PATH>
OneFileEncryptDecrypt encrypt --password <PASSWORD> --file <ENCRYPT_FILE_PATH>
```

### Decrypt

- Decrypt file

```bash
OneFileEncryptDecrypt decrypt -p <PASSWORD> -f <ENCRYPT_FILE_PATH>
OneFileEncryptDecrypt decrypt --password <PASSWORD> --file <ENCRYPT_FILE_PATH>
```

### ExportSalt

- Export crypto salt.

```bash
OneFileEncryptDecrypt exportsalt -d <EXPORT_DIRECTORY_PATH>
OneFileEncryptDecrypt exportsalt --directory <EXPORT_DIRECTORY_PATH>
```

### ImportSalt

- Import crypto salt.

```bash
OneFileEncryptDecrypt exportsalt -f <EXPORTED_SALT_FILEPATH>
OneFileEncryptDecrypt exportsalt --file <EXPORTED_SALT_FILEPATH>
```

## Check Point.

### Crypto Salt Location

Crypto salt stored system specific location.

Default location is application sub directory.

If you change location?

`exportsalt` after check to `appsettings.json` in `crypto.workDirectoryPath` value.

- Sub directory (Default)

```json
{
	"crypto": {
		"workDirectoryPath": "."
	}
}
```
- Specific location

```json
{
	"crypto": {
        "workDirectoryPath": "C:\\Dir1\\Dir2"
	}
}
```

After change  `appsettings.json` in `crypto.workDirectoryPath` value.

Require execute `createsalt` or `importsalt`.




