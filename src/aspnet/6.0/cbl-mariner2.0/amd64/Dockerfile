ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.10-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.10

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='f3d523c0544437ff460ea5164db4417eacb834d70dce40a26f7869e1e88b1f4453cd2ddadabe4ee80845fd7fe58ffb474e1e9062f92aeb1061edb82ad52cc14b' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
