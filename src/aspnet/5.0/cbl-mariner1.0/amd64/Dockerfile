ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.12-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.12

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='a5fc442c7edb213a43e6e7a161dd451684edadb1f25b0a97cca39c914c856142fae766299d54ba10902eb18810cd4c7576b24e51604b99419736f9939aa6978a' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
