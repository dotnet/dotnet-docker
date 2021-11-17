ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21562.1/dotnet-runtime-7.0.0-alpha.1.21562.1-linux-arm64.tar.gz \
    && dotnet_sha512='ae00de8257f127ea5e05c6941335fd1b4141615dfe7ca78ae9ee73ce007a476aed3f4e13c2746529cf9543249b95d77648c3d4a8e17f0264d225195c85b73853' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.21562.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
