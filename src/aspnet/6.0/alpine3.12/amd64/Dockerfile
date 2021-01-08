ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 6.0.0-alpha.1.21057.7

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='45e2733625205e8d6b7376b1b4ea7b05660bad0e5b909ed28492995b910e7835cd50bc2ab990a358181e177edcbecb74217784f487ce14b3421f1e18207dbcae' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
