ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.11-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.11

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='5d50308db196f1aae85c09e30c0b372ef38c224e39250461c716c110e569337516bb0674467d6fa821df3b82167406c4c05f61147b17b5c6ab4d3ab15c9ef1b5' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
