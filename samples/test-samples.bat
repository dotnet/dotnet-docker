@echo off
dotnet tool restore
dotnet pwsh ./test-samples.ps1
