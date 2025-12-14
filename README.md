# SpiderMem Backend – Quick Start Guide

## Prerequisites

* **Git** installed
* **PowerShell** (Windows default)
* **Docker Desktop** (or Podman: https://podman.io/)

---

## 1. Clone the Repository

```bash
git clone https://github.com/martrzeciak/spider-mem-backend.git
cd spider-mem-backend
```

---

## 2. Install .NET SDK (PowerShell)

```powershell
.\install-sdk.ps1
```

**Expected output:**

```text
Required SDK: 10.0.101
Installed SDKs:
10.0.101 [C:\Program Files\dotnet\sdk]
SDK 10.0.101 is installed
```

> 🔄 Restart the terminal after installation (close and reopen).

---

## 3. Verify Setup

```powershell
dotnet --version
# Should show: 10.0.101 (matches global.json)
```

---

## 4. Run the Application

### Option A: From solution root (specify project)

```powershell
dotnet watch --project src/SpiderMem.API run
```

### Option B: Navigate to API project (recommended)

```powershell
cd src/SpiderMem.API
dotnet watch run
```

✅ Backend will start at:

* [https://localhost:5001](https://localhost:5001)

---

## 5. Docker

```bash
docker compose up -d
```

---

## Visual Studio Setup

1. Open **SpiderMem.slnx** in Visual Studio
2. Run the SDK installer in VS Terminal:

   * **Terminal → New Terminal**
   * Execute:

     ```powershell
     .\install-sdk.ps1
     ```

---

## Troubleshooting

```text
❌ "Could not find MSBuild project"
→ cd src/SpiderMem.API && dotnet watch run

❌ "SDK not found"
→ .\install-sdk.ps1 (then restart terminal)
```

---

## Project Structure

```text
spider-mem-backend/
├── SpiderMem.slnx             # ← Open this in VS
├── global.json                # Pins SDK version
├── install-sdk.ps1            # Auto-installs SDK
├── src/
│   └── SpiderMem.API/         # ← Backend API (dotnet watch run)
│       └── SpiderMem.API.csproj
└── docker-compose.yml         # Docker setup
```
