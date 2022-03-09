ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.3-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='aa7b42e59d81efecc3353f06d6c39e908536bbdc607a78222e2e2bfa72197e3cb18cf528a086bb52b09ff9681728b23ca01e27722f03d3870d0c800ae1fdd5e4' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
