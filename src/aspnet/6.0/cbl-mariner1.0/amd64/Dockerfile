ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='b22316d2e0950ac56cc423cfd70115ca1bc263344c6a36bad0e9842c7304845d99602bbb1e08783d86dafb17e607eb893dfd2c9c2bdb7103b018173918143982' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
