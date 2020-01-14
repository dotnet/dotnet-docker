@echo off
dotnet tool restore
dotnet pwsh ./build-and-test.ps1
