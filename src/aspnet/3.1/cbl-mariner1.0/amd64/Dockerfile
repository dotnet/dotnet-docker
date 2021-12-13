ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.22-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.22 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='549ace9cc921d38b1ef23096efa67220115a0cf55386c51ef7f386dc0b7a9cbb8bb69a9299c96ab1e29b72d2137503c04e4b1bc7c5e5bd84e130e087364b91dc' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
