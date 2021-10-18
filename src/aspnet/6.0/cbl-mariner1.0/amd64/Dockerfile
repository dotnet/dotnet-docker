ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21517.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='7edfb1a53c0745172fb1757fad532227bd77676892b16a18bdda552737b6ac8d55e5a67df3f55640b065ea4ada12d347c3b15c569759072e99fa1f11a4a098fa' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
