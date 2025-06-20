ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.6-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.6


# Install ASP.NET Composite Runtime
RUN wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='4b36300b9d40feb9de34468898780c4f1a38f2bacb2664ab3fcba11aa55282e329cba0deb09704c0ff5f4e0d713000533fcaf8f0a34ac4cde4833bb3224de782' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
