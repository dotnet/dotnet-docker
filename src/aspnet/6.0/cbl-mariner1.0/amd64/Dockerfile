ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.12-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.12

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='07b516c92dd05a8c1762673df920ba1d037904dd9fb8350aa2ffac7f9e73b14e538b2c543787ee2f843891297f54c936ed587318654c69d87d2526b7af00308d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
