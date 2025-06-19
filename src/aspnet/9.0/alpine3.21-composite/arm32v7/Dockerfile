ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.6-alpine3.21-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.6


# Install ASP.NET Composite Runtime
RUN wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='9f1e39593bd4846574435bb7f492e3b5930ce7c3eb064437635fa207a130ce2283ebea08284141191c7635c67ae4e791dac55e7560dec1f38695056323e83f64' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir -p /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
