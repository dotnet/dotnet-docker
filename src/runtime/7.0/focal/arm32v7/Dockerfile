ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21567.1/dotnet-runtime-7.0.0-alpha.1.21567.1-linux-arm.tar.gz \
    && dotnet_sha512='ad6bbcdca2c79fd247c8eed877d4165ab39502c20b75f42f6d0febafb3ef2bba73edce07bb128ca88735b70e14a025b1d4b3f49cbae1463ab0cc1c4a897054b9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.21567.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
