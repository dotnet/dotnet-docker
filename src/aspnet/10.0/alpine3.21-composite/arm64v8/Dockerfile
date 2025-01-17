ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.1-alpine3.21-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.1 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.1


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='5de05d1c2a12a4276d788295a78484360f2a65a99dab935469696bc8588d8a23a10ccdbc777df9dc9445dda7541e33f4f583ef7b00d06cfbcdfd43339cffa809' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
