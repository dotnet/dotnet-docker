ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.25-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.25

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='9573d1f435b40f853c95b2ba846f4ad6a004b8a9761acbceb28ff0ae350f8676703c67937749c310a1f5a6ea57dd112460a8406c236880ad350b748b4df1ef9d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
