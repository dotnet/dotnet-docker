ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.24-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.24

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-aarch64.rpm \
    && aspnetcore_sha512='700f64a39e96aa2be794dbe476dcfc4bd30eb63951f0c92f7c87718ecd1f83b87c117c939566140dedcc56529e34441c1127ba94f21ef0795d1bb9cc3bdd4aee' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
