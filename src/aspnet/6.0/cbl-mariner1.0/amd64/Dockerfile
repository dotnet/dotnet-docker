ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-rc.2-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21480.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='04d9481de1ea8844e23001995867140fb383f6ba09be206fa04b7ea96437fd87184484c7a4ddb13d633e522f616a44455cc4870bc720bd5ef91f436b6218c778' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
