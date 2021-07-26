ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.7-alpine3.14-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21374.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='40f7708cf7fb98c2419b89f5d0d087bb507d1aa39cad37b113efead6f809db941a058961d3cb0c0a23c19c6d6df1b1c7f1934d320fce55f75340f2defa0adea3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
