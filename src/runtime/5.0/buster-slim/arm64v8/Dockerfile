ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:buster-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=5.0.14 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='4de448bfe995042aec23a280ea1c5df53b5ebd2702da32885d5d9fd06fd64d06a0eee53fabfe4d21a40e2bdd9c01b69e8defe7697b0cb12fc052e2d1e7d3d822' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.14-buster-slim-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=5.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
