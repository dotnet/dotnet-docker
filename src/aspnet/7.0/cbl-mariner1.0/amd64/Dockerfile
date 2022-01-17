ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22066.1 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='668c405cecde4f1d9272f967a0a6e389636064c314c0a7a709aa1c90e0309649e90151738af6c2afbb38be879f2eebd587a2e27645335cf7483a9230973a8181' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
