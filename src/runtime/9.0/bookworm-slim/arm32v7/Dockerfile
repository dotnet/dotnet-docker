ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.4.24223.11 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='18638e25866788cfabbe36dc9e1e21c2846a072f9b6956c6f54138b4a879f91ed0b4ba897e68215c718d0797c39dc5f46e81ee951364c299cb70b4d042cc49b4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.4-bookworm-slim-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.4.24223.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
