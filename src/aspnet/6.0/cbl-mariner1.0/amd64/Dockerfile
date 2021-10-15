ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21514.23 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='e21842fe1fef5a130af3eda87823f90c47efc9b5a761114fae83d71b9ccfd0d1e27123a251b03bea3d6669653c929a91ed33b559ba3b5a5fc66b0f809a234583' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
