ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21325.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='1fb369196d77fe5c609e99ac3a2e07b8f61b3f230fc69824addf7680fed5325609efe47a5902b60b8bf292cab5271a32d39adb4f60f1be0d57bb692f02f1f0ab' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
