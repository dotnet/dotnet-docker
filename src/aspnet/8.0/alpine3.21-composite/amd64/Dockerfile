ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.13-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.13 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.13


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='7c29859c689d3dfee555f6dd43ba6b9b83ba8446c6f4457ee47976cc41da0d53f889283b063786426d485f2751ccc2a58b6a0dcc6b18309961ae02ee67d10c63' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
