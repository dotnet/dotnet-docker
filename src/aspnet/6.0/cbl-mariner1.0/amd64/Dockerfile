ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.2-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='96fe278eaf522ada29604f0f2a0ab7f8c8ab84eba1e38c930ba8f6f6f80e7f45669d3085382373ab758317f036dfdf33de4e3db716ea6a9ce270484c5f0ef6b2' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
