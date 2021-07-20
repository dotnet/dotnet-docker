ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.7-alpine3.14-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21369.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='420e96c0d5878715207f2b2dd80a3885656f2c8af0e5b4206d4b3057e74e43c6978f9e4ad8828fe1270255d1d41ddf0287cbfe1486c510ae6736c9dda7f6cade' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
