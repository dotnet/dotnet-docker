# build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env

WORKDIR /update-dependencies

# copy csproj and restore as distinct layers
COPY eng/update-dependencies/*.csproj ./
COPY eng/update-dependencies/NuGet.config ./
RUN dotnet restore

# copy everything else and build
COPY eng/update-dependencies/. ./
RUN dotnet publish -c Release -o out --no-restore


# runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim

# install git
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        git \
    && rm -rf /var/lib/apt/lists/*

# install Docker
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        apt-transport-https \
        gnupg2 \
        software-properties-common \
    && curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - \
    && add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian $(lsb_release -cs) stable" \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        docker-ce=17.12.0~ce-0~debian \
    && rm -rf /var/lib/apt/lists/*

# install PowerShell
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        less \
        liblttng-ust0 \
        locales \
    && rm -rf /var/lib/apt/lists/* \
    && curl -L https://github.com/PowerShell/PowerShell/releases/download/v7.0.0-rc.2/powershell-7.0.0-rc.2-linux-x64.tar.gz -o /tmp/powershell.tar.gz \
    && mkdir -p /opt/microsoft/powershell/7-preview \
    && tar zxf /tmp/powershell.tar.gz -C /opt/microsoft/powershell/7-preview \
    && chmod +x /opt/microsoft/powershell/7-preview/pwsh \
    && ln -s /opt/microsoft/powershell/7-preview/pwsh /usr/bin/pwsh

# copy update-dependencies
WORKDIR /update-dependencies
COPY --from=build-env /update-dependencies/out ./

# copy repo
WORKDIR /repo
COPY . ./

ENTRYPOINT ["dotnet", "/update-dependencies/update-dependencies.dll"]
