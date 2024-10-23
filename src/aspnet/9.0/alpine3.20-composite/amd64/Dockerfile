ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-alpine3.20-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rtm.24516.10 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rtm.24516.19


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='f56517cecc2037b6c8feeaea5643dca4531ea9103eb76716907890a1a1ab82392c1bbb1262e6512ffc7ee3db0676fece003d732c0980cd69a241634bd4a4dc5d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
