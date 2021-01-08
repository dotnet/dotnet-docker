ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.20629.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ebc5e7eb17a703ac703d3dcccd0003b3bea69234244a4de9494a2efd839ab93a3ceb0950f49dfe964c6fe71854cdf79c8f9644a39b206b431a26fe13bf5165f1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
