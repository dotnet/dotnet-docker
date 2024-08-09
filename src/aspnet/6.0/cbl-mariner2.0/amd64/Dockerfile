ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.33-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.33

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='a06db785296112dd95b60ad5d6eb5395c198ffa6132a08831f61ff4c4e79ab0d38c83387c4cc36b1f13b10b8338def8134ba20d1a874932c01e431fcb90f7bde' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
