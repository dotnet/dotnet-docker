ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.26-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.26 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='0d7762a8646fc40e140835dd62a60523b43595c640914b03c987f608654866006eb08fdc619302629ae6bdf77d92bc52e1940fb10b70f9e76336a4a1fbc1f76e' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
