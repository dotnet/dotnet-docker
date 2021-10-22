ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21521.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='9373a016936eb632eaf85b224b2255bb86545b020b8f10feb860190f59f635e7a9abf39b4b36a8cbbc716319f2092c952881d0b7041f421cf05f3b7e0cb203cc' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
