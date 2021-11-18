ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.21567.15 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='af489a5701c3e0a911b2079adf169cf888cd460428bf219b0682a3be56ca92bf47ad2d43359cd38621ebd625bb5e9676af99fda4e67bb4bf846ebdf179521a3d' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
