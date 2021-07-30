ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.17-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='69124446aede08ec8876ac6ab2ddf0e561479d9117217295d973c4ccebe8da7e82485df43c448f720eb1d2bfdb12879c88f4eacfbd1be3d8bb242178b6cdf409' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
