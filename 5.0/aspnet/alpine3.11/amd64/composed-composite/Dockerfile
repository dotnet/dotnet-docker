# TODO: define a shared base stage (build infra doesn't support)
ARG REPO=mcr.microsoft.com/dotnet/core/runtime


# Installer image
FROM $REPO:5.0-alpine3.11-composite as installer

# Install crossgen2
RUN wget -O crossgen2.tar.gz https://dotnetbinaries.blob.core.windows.net/dockerassets/microsoft.netcore.app.crossgen2.linux-musl-x64.$DOTNET_VERSION.tar.gz \
    && crossgen2_sha512='6beecdd14e52d48b8e794b73dab4b148a50744c1a50f1108c1c30bc1df20fb2cd1741359e7a3f3803e8576ef166308b0d558a18ea6456b343586056025b63e1c' \
    && echo "$crossgen2_sha512  crossgen2.tar.gz" | sha512sum -c - \
    && mkdir /crossgen2 \
    && tar -C /crossgen2 -oxzf crossgen2.tar.gz \
    && rm crossgen2.tar.gz

ENV ASPNET_VERSION 5.0.0-preview.2.20167.3

RUN \
    # Retrieve ASP.NET
    wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    &&aspnetcore_sha512='029ed32f8a7dfdb258438177b7fd40c187830db35b3d973dec296d8948f99d30a74269c45d31876c9b4533b8d7b6af65dea319299bd9a7081c6881cf026274b4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz \
    \
    # Crossgen .NET Core
    && /crossgen2/crossgen2 \
        --composite --inputbubble -O --Os \
        -r /usr/share/dotnet/shared/Microsoft.NETCore.App/$DOTNET_VERSION/*.dll \
        -o /usr/share/dotnet/shared/Microsoft.AspNetCore.App/$ASPNET_VERSION/Microsoft.AspNetCore.App.SharedFramework.dll \
        /usr/share/dotnet/shared/Microsoft.AspNetCore.App/$ASPNET_VERSION/*.dll


# ASP.NET Core image
FROM $REPO:5.0-alpine3.11-composite

ENV ASPNET_VERSION 5.0.0-preview.2.20167.3

COPY --from=installer ["/usr/share/dotnet/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
