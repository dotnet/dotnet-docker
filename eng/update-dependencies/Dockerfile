# build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env

WORKDIR /update-dependencies

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY eng/update-dependencies/NuGet.config ./
RUN dotnet restore -r alpine.3.9-x64

# copy everything else and build
COPY eng/update-dependencies/. ./

# Need to publish with version-specific RID. See https://github.com/libgit2/libgit2sharp/issues/1798
RUN dotnet publish -c Release -o out --no-restore -r alpine.3.9-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true


# runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine

# install git and Docker
RUN apk add --no-cache \
        docker \
        git

# install PowerShell
RUN wget -O powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.0.3/powershell-7.0.3-linux-alpine-x64.tar.gz \
    && echo "18b0e1e5286559163ba121039e2a747c14105796c673f126498ecff22d46b92b  powershell.tar.gz" | sha256sum -c - \
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
