ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21414.4/dotnet-runtime-6.0.0-rc.1.21414.4-linux-arm64.tar.gz \
    && dotnet_sha512='8457547426ad211d2d1f3ce50302f7c18e7318b8f4c347218af54591c13d9b40068bc9779b089a8435a499b963187123215ac2183a1d593e4726ddb5da391c1b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21414.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
