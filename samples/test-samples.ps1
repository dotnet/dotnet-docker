#!/usr/bin/env pwsh

./build-test-images.ps1 dotnetapp 1
./build-test-images.ps1 ./complexapp 1
./build-test-images.ps1 ./aspnetapp
./complexapp/build-and-test.ps1 -path ./complexapp
