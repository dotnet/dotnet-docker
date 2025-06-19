ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.6.25274.104 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.6.25274.104


# Install ASP.NET Composite Runtime
RUN wget --output-document dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='ecd7f3c809fdcf08ab1674d19f389156d3a5751db578169b7dd99a578d9eda692793b91f012c68aaf2922dd4a47e9df6206fff38f26c65e9c5a9a05a8a2c3bc6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln --symbolic /usr/share/dotnet/dotnet /usr/bin/dotnet
