ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=6.0.14 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='4f559d5da668c67ed61c3e342e5ca77f2059b45bfa84f8c35b5ab4d5acb56ce75daf05713ef7b3ce299c1084fc863d5e43a4c14b1e04ce86db084b1fdd465a1c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.14-focal-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
