ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.20-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.20 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='5def0a3a803bdc7103f72f2856c38e9fa224a93083852b36b657d1cc0e058a6d6990c6dfa182ef206d56bdd6d50d0c4e43e8417728037c07533f1acb2459e763' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
