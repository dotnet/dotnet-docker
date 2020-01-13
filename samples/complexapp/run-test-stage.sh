#!/usr/bin/env bash
dotnet tool restore
dotnet pwsh ./run-test-stage.ps1
