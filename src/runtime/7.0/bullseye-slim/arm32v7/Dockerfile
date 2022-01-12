ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22059.2/dotnet-runtime-7.0.0-alpha.1.22059.2-linux-arm.tar.gz \
    && dotnet_sha512='dde62962d6bb33aef879ef8de4e469cea167cb8d0601bf27785f0ba5430d24ff2c0288eac1d17bb0aaa23f90611e18c0507a7e138a87299554bb09c48be15a64' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.22059.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
