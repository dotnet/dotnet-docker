# build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env

WORKDIR /update-dependencies

# Need to publish with version-specific RID. See https://github.com/libgit2/libgit2sharp/issues/1798
ARG RID=alpine.3.14-x64

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY eng/update-dependencies/NuGet.config ./
RUN dotnet restore -r $RID

# copy everything else and build
COPY eng/update-dependencies/. ./

RUN dotnet publish -c Release -o out --no-restore -r $RID --self-contained /p:PublishSingleFile=true /p:PublishTrimmed=true


# runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine

# install git and Docker
RUN apk add --no-cache \
        docker \
        git

# install PowerShell
RUN wget -O powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.2.0/powershell-7.2.0-linux-alpine-x64.tar.gz  \
    && echo "dccf213728b6d539785cbcc9ad9bc0971d82c7e8cf010e8f949e3dd17fc4c596  powershell.tar.gz" | sha256sum -c - \
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
