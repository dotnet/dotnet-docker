# TODO: define a shared base stage (build infra doesn't support)
ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps


# Installer image
FROM $REPO:5.0-alpine3.11 as installer

ENV DOTNET_VERSION 5.0.0-preview.2.20160.6

# Install crossgen2
RUN wget -O crossgen2.tar.gz https://dotnetbinaries.blob.core.windows.net/dockerassets/microsoft.netcore.app.crossgen2.linux-musl-x64.$DOTNET_VERSION.tar.gz \
    && crossgen2_sha512='6beecdd14e52d48b8e794b73dab4b148a50744c1a50f1108c1c30bc1df20fb2cd1741359e7a3f3803e8576ef166308b0d558a18ea6456b343586056025b63e1c' \
    && echo "$crossgen2_sha512  crossgen2.tar.gz" | sha512sum -c - \
    && mkdir /crossgen2 \
    && tar -C /crossgen2 -oxzf crossgen2.tar.gz \
    && rm crossgen2.tar.gz


RUN \
    # Retrieve .NET
    wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='f8dcf9f498feffbca4b0649086cffd298cd98189b2950c32dd52b07179b7f33e1acfe14ae59a266502a3518089ca55f224e007628d16194f94be7dc419c1b2b4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir /dotnet \
    && tar -C /dotnet -oxzf dotnet.tar.gz \
    && rm dotnet.tar.gz \
    \
    # Crossgen .NET
    && /crossgen2/crossgen2 \
        --composite --inputbubble -O --Os \
        -o /dotnet/shared/Microsoft.NETCore.App/$DOTNET_VERSION/Microsoft.NETCore.App.SharedFramework.dll \
        /dotnet/shared/Microsoft.NETCore.App/$DOTNET_VERSION/*.dll


# .NET runtime image
FROM $REPO:5.0-alpine3.11

ENV DOTNET_VERSION 5.0.0-preview.2.20160.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
