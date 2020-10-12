ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rc.1.20451.17

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='963ad9dbaa48e1224d53b7db3d8869de973e2d576368cb87ba95f3fafaa49cf36e1988eecaafe1e0efbd36f2cd3d9d171c39c5205ad93cdcdcf5a7c13860cef9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
