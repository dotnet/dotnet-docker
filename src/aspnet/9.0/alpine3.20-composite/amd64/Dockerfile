ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-rc.1-alpine3.20-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rc.1.24431.7 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rc.1.24452.1


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='33ddf922b107124112d4cb42efe040d04299aabe77e0cf47791e0e7ee9ccd106bc5c56cf792b49965570a0cb55ff40693e20eb9e3b868557b53000a091a9adaf' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
