ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.1-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.1 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='1c9e5bb3e578af73b8eda17dbb8e098cead7c9e3e8adbb7582c51fa98746856e8ffd4889ca3b5e7f9fe6337e6ca1a1f783bfced401848653b4dbfa142ad815b5' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
