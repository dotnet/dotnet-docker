ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21519.6 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='61b4cd51644fbd82b04e03bf26cd55bcf9f9b13756f8ccb59b51234724251af27e7e77dc688e3562695f1f6e5194f8e6cdb5e3008aab0fe886a311bbe7d3772e' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
