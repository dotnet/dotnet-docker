ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.5-alpine3.22-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114


# Install ASP.NET Composite Runtime
RUN wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='246f726e68beacceb8b96177663bb2d6b00153ed51bfe591234f5f59f9c8e50b4df3c8e8084c52d81ff144d47fb78bd7c40e7c9bdd51b7dd4ee51aa5ed462f30' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
