ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-alpine3.14-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21523.4/aspnetcore-runtime-6.0.0-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='f876733c457e2c01950e61056fe4b5f6ea4f82e675d9f8e98ed36bdc279e077060d7ede2b656ecad9ca98648c995eb366bc9d2dc1c0e611cd1d3257407862387' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
