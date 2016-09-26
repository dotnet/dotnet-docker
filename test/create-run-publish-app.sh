#!/usr/bin/env bash
set -e 	# Exit immediately upon failure

: ${1?"Need to pass sandbox directory as argument"}

cd $1

echo "Testing framework-dependent deployment"
dotnet new
dotnet restore
dotnet run
dotnet publish -o publish/framework-dependent

echo "Testing self-contained deployment"
cp project.json project.json.bak
runtimes_section="  },\n  \"runtimes\": {\n    \"debian.8-x64\": {}\n  }"
sed -i '/"type": "platform"/d' ./project.json
sed -i "s/^  }$/${runtimes_section}/" ./project.json

dotnet restore
dotnet run
dotnet publish -o publish/self-contained

# Restore project.json to specify framework-dependent deployment for the onbuild image test
mv -f project.json.bak project.json
