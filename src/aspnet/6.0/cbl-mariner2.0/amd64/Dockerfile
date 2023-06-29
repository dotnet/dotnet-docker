ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.19-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.19

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='db9de013810d716937bbeeab13606f9f6e830230c2344dfd0a3bc139235e78586f3ef4e7644a9eee219d06f8b0f6e091504cdd8c1773fc0e80a3844a080691a8' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
