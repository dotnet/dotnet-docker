ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-preview.6-alpine3.17-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.6.23280.5 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.6.23280.11


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='af8202e8e1238b6e1bb4f033f10b6e4b6edde3da14317fd4745286475232ef7f8be16e3c13417818fa4773dbe642ea85410a55c442bb3691eec1265d4ccd7299' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

