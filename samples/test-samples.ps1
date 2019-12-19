#!/usr/bin/env pwsh

./build-images.ps1 dotnetapp 1
./build-images.ps1 ./complexapp 1
./build-images.ps1 ./aspnetapp
./complexapp/build-and-test.ps1 -path ./complexapp
