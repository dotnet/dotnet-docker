ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.11-alpine3.21-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.11 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.11


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='74c72b06059b51d75fb05e6495a65f096ed39d6c3ebcd0e3fb4712873d6effeb005d85e1296f334acbe32ce53e3ace66960e756b1e3f7155257508eb9c8295eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
