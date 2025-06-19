ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.17-alpine3.22-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.17 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.17


# Install ASP.NET Composite Runtime
RUN wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='37d8ad76aac48aa9e064bbd155a6c5db0117f42bac0530b676956845b74d59a6888c0af39f9f7d1b8a9a317f0b58f421948eef8b3545d91979b45844a85f2a84' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum --check - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln --symbolic /usr/share/dotnet/dotnet /usr/bin/dotnet
