ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21526.8/aspnetcore-runtime-6.0.0-x64.rpm \
    && dotnet_sha512='24457431baca7e8dfe9e2289278891be8be943cd6561194f5895944c5ef843bdf5921ecfb55a2501698599e2c182e467adc3f60f85c2bb9173dd733486687b2b' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
