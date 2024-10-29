ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-rtm.24515.9 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='0df2d602e7da02e70dec625221ec3cd6984294e40328702f80bab39a5f8fbdc02025359f941d1204bb4d9947d0f76333e71642808b48fa43d46b7e2a9d5ae0ba' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-noble-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-rtm.24515.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
