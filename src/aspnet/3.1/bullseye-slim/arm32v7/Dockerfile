ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.19-bullseye-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.19 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='d28f77adde711ac1eea40de702e47340c6021cd2ab529fc5009b0662de3ae0dff1668427975072776bef7bbe4d85df2b42e116e66e542d8e668505e6d9b2782c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
