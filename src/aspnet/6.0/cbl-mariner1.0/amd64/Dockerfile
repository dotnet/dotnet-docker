ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21451.36 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='18a19aeb8cb3387edd409496d8d35850184089fed4c1241ffa54092e0482f74cc8c3e6d7ae96295e4dfc301a2479ef3b81d9920819ca0b2837886851dee315cf' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
