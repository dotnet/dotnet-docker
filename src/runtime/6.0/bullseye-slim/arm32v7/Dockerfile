ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21430.11/dotnet-runtime-6.0.0-rc.1.21430.11-linux-arm.tar.gz \
    && dotnet_sha512='c142321f6643d56561cebf3adfbd9216ca8280023328db43310b1f00a0b982f084df46034c6ab932470bd1a85ab8e40581a400c8ef5fdab9b4bd387f188bb276' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21430.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
