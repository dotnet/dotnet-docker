ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=5.0.0-rc.1.20431.14

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='b038c134bd3eae48a4eddac04ab707a19dfb7be9b89d392337ef5997b6e1acfe029439c5b425a1de00cd7bd61bc9a1275a1851204c76f335619ca94cad7378d2' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0-buster-slim-arm64v8
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
