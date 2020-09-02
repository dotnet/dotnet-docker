ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
ARG DOTNET_VERSION=5.0.0-rc.1.20431.14

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG DOTNET_VERSION

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='3ef7f4d223202ec92b839cb221cd1fc5e135c15862770347a17ce393271e7d3b15e6ae91bfe197e5196f6276c00b56b86205bf4143a853a9c1524e572f59eb08' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0-focal-arm32v7
ARG DOTNET_VERSION

ENV DOTNET_VERSION $DOTNET_VERSION

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
