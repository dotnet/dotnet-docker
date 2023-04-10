ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.16-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.16

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='cd38e6f36e0b8ad450e798dd7b7e2b3e3506bb90f8ca6b42c86a386172c9e77ec9d11318f0b45c4005b00c1acfb40375208e7529f46529200c6449f085ec3425' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
