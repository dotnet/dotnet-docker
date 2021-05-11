ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.15 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='3c4fcbf9eab630f25605cfcbe8c55af452d4b10cc054257e26dfa4bdb09fd025c35cc9ea12dbcbc7c420946e618b9e8ba50ca81525b5055c79323bb7e94b5544' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
