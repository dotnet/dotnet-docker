ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.17-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.17

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='29cd45a43067a971604ef866334f5484a49af42af6f46960384abaf908989f6eaf042a0456161dd28e96920c5ef022ff757b859e0233eaa8632862220dd2e927' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
