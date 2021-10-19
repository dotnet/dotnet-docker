ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.18 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='d09d795c73631eaae53804b98f9f26718f30850efc9e0bccfc5475519e99881f9c905ed4ee839e679c255f38dac47f9ad79ec9adf5062a4f7829384330fadf16' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
