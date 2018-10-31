# escape=`

# installer image
FROM microsoft/windowsservercore:1709 AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# retrieve git
RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; `
    Invoke-WebRequest -UseBasicParsing -outfile git.zip https://github.com/git-for-windows/git/releases/download/v2.16.1.windows.1/MinGit-2.16.1-64-bit.zip; `
    Expand-Archive git.zip -DestinationPath git; `
    Remove-Item -Force git.zip

# retrieve PowerShell
RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; `
    Invoke-WebRequest -UseBasicParsing -outfile powershell.zip https://github.com/PowerShell/PowerShell/releases/download/v6.1.0/PowerShell-6.1.0-win-x64.zip; `
    Expand-Archive powershell.zip -DestinationPath PowerShell; `
    Remove-Item -Force powershell.zip


# build image
FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build-env
WORKDIR /update-dependencies

# copy csproj and restore as distinct layers
COPY scripts/update-dependencies/*.csproj ./
COPY scripts/update-dependencies/NuGet.config ./
RUN dotnet restore

# copy everything else and build
COPY scripts/update-dependencies/. ./
RUN dotnet publish -c Release -o out --no-restore


# runtime image
FROM microsoft/dotnet:2.1-runtime-nanoserver-1709

# install git
COPY --from=installer-env ["git", "C:\\Program Files\\git"]

# install PowerShell
COPY --from=installer-env ["PowerShell", "C:\\Program Files\\PowerShell"]

USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\git\cmd;C:\\Program Files\\PowerShell"
USER ContainerUser

# copy update-dependencies
WORKDIR /update-dependencies
COPY --from=build-env /update-dependencies/out ./

# copy repo
WORKDIR /repo
COPY . ./

ENTRYPOINT ["dotnet", "/update-dependencies/update-dependencies.dll"]
