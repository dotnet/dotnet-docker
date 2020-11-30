# build image
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env

WORKDIR /update-dependencies

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY eng/update-dependencies/NuGet.config ./
RUN dotnet restore -r alpine.3.12-x64

# copy everything else and build
COPY eng/update-dependencies/. ./

# Need to publish with version-specific RID. See https://github.com/libgit2/libgit2sharp/issues/1798
RUN dotnet publish -c Release -o out --no-restore -r alpine.3.12-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true


# runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine

# install git and Docker
RUN apk add --no-cache \
        docker \
        git

# install PowerShell
RUN wget -O powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.1.0/powershell-7.1.0-linux-alpine-x64.tar.gz  \
    && echo "0b4405505eb97d4821449657a99a3da4aa75f270f3a7745b451b461f2eadb39d  powershell.tar.gz" | sha256sum -c - \
    && mkdir -p /opt/microsoft/powershell/7 \
    && tar zxf powershell.tar.gz -C /opt/microsoft/powershell/7 \
    && chmod +x /opt/microsoft/powershell/7/pwsh \
    && ln -s /opt/microsoft/powershell/7/pwsh /usr/bin/pwsh \
    && rm powershell.tar.gz

# copy update-dependencies
WORKDIR /update-dependencies
COPY --from=build-env /update-dependencies/out ./

# copy repo
WORKDIR /repo
COPY . ./

ENTRYPOINT ["/update-dependencies/update-dependencies"]
