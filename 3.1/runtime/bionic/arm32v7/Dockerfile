ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-bionic-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=3.1.1-servicing.19575.1 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='09f30b79935834cdb59f872667bc7000957136492f93a782c967a56632a6ca15c303713ff544327101cd020b5a58c60cb6d4a5a313d1e000dd2a93e7d6944647' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
