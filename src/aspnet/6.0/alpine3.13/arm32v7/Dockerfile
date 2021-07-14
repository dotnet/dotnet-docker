ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.6-alpine3.13-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21355.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='50f1c68f64aeb576452ddf11a26e5e2bbb9d713ebb9bead6f8d73b16b8111eafec5841c470332a8700ebcd7234b87d453751857fc93ec6cc20a5fcdbbe42c6ef' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
