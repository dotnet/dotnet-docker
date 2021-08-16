ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-rc.1-alpine3.13-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21413.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='c218ff830b76689d8de771f39107b90311d3ae81a76ba5c3d2ca98ef7c315a958c4078fbb874999895d73fd21c8aab0d80aa7fae956cb1cd4f06600e26068a19' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
