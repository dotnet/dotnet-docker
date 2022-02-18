ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22117.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='b8a9799ab1835c1ec17ca252dc9c63e6fb30be7fda1f828fc64af6a29f645a3a77048f5feb3c82eac6558f279e68bdeb9593a63450358bade796e7f891323141' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
