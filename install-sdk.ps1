$version = (Get-Content global.json | ConvertFrom-Json).sdk.version
Write-Host "Required SDK: $version"

Write-Host "Installed SDKs:"
dotnet --list-sdks

$installed = dotnet --list-sdks | Select-String $version
if ($installed) {
    Write-Host "SDK $version is installed"
} else {
    Write-Host "SDK $version is missing"
    Write-Host "Installing SDK $version..."
    Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile "dotnet-install.ps1"
    .\dotnet-install.ps1 -Version $version -InstallDir "$env:ProgramFiles\dotnet"
    Remove-Item dotnet-install.ps1
    Write-Host "SDK $version installed. Restart your terminal."
}
