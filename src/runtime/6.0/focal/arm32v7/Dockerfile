ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21421.1/dotnet-runtime-6.0.0-rc.1.21421.1-linux-arm.tar.gz \
    && dotnet_sha512='49a0d227d1c2366c3f0528f93fffb4ce79f40ccfc74a5bf7c5f179503c2e07c18f06256f8ca683f604fd59d587a3889b28defcd1fbfd82f7a3ed4cdbb933accd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21421.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
