ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.1.22076.8 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='90bec2461459694b0d6e430066171ae40669690920f2db2542f4c0dba51465a1e402dea21c24c0917626666e0e4d41512ab4f9cc157c30f42c7f8d1b8b4962cf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.1-focal-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.1.22076.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
