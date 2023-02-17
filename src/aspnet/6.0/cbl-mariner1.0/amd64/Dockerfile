ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.14-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.14

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='92e60723e1db155efe4f6c4a0f81cdd87033bdf1b67c660211a00b57f3a603d6a10dd18faf0f1b7d220107f65cebf0c31286dd2265fffd23f4c1257a53c097f4' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
