ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.11-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.11

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='f890da7fb82e3740bcf7a86e79f94c96a0e870b45b78056778126436e17e54a8549ee08dba655a079499204e9f6e9f0f5d20c517827f073b08f484ae21c977cb' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
