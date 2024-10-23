ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-alpine3.20-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rtm.24516.10 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rtm.24516.19


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='f613e4aabdeaaffac15dc2c17c5695d97245cc63dad437bb03a3fd0d8d991028e1f807f2a567ae48b6528d98961349cbbe4fd1bab73777bf3f928a83095a0cea' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
