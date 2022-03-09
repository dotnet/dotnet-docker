ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.15-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.15

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='33933432b92036a845174a80ac32e11eaef8ee76da99b1389d950560361999cb4096e77d6e94eb30eaae5ea953949cc1c2e4c6fe29af7ecc254cc0d79a14c291' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
