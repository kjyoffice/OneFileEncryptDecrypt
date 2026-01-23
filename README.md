# OneFileEncryptDecrypt

### One file encrypt decrypt in my machine.

### Protect my important file.

## Require

.NET 10

## Optional

VisualStudio 2026 or Higher

VisualStudio Code with C# Extension

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

### Network

Encrypted file send/share to another machine is not recommended.

First concept is `MY MACHINE` in file encrypt and decrypt.

If you know encrypted file send/share network risk.

Please `MY ANOTHER MACHINE` or `TRUST MACHINE`.

## Change Log

### 2026-01-23

Delete system salt.

Every encrypt time create new salt use.

And salt is include encrypt result file.




