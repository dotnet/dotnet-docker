#!/usr/bin/env bash
dotnet tool restore
dotnet pwsh ./test-samples.ps1
