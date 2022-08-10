ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.8-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.8

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='33b6f5c0cb1c9c2f0dab10c258812efb598a854329bf9b4e172b6c704f9604afbe1d36462bb513844b40140d7febd815f12ff54d05477853e52277d39f60b1ed' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
