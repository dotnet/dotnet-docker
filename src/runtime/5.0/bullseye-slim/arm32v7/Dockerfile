ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=5.0.14 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='c6fa8c0218cdd98fee31cc24374f161c85a49415f21e227dfa24944e45fdff4087f6d865d21f0e2105618d76e95d2153d16a9521315a21e854ee3b0522c366a6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.14-bullseye-slim-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=5.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
