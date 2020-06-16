ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.5 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='262a8e670a8800aea1c518e48a237543f2bca92010187d25cae2bd513163786c5b49ff2593b1e256ca89201fd3d819c2265f8a3946b257e8490b37a5a66e1fff' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
