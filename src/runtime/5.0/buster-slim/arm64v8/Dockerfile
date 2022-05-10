ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:buster-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=5.0.17 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='99cb11871924d3abedcc9c8079c54bc0c550203c7cbe4e349ed70d4355f40e4620b68d90b797e6461d898c06bed6953580e2cd4ad01483e5de107ca5a3409610' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.17-buster-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=5.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
