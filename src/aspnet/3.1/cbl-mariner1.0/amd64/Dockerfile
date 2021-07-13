ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='456cdf2eaa85ab6eeab6555072bd472090fdd2aa20bf830c13b33976c3cac02570bf573ba08098d0303746203787d227e716fd417f94cc04a6256528e2459f97' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
