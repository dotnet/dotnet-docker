ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21365.18 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='49d0ab0accf40ad874f9c26af4d0699cee6420740787351e69f2a40abf1f10116861780a77108a01bdebdcd037c95fbcf8edb0f3e6c0835a7c52d231a43ace7f' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
