ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21411.2/dotnet-runtime-6.0.0-rc.1.21411.2-linux-x64.tar.gz \
    && dotnet_sha512='cba9c424078a86afaec527ee58f93a441fed6680d0c94d4dc2caed7c45c2bc2b394f3ab8f9fc74ae57aec8ef4097a2b239f6c49206e272c8e1b17398d34477f3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21411.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
