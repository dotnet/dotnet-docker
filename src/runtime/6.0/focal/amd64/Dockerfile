ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21370.18/dotnet-runtime-6.0.0-preview.7.21370.18-linux-x64.tar.gz \
    && dotnet_sha512='7dff8bf860e76df80a4f6eb1a882c7325f8feadc4cee39bd752557c8362180179fc858f7ae081c6d90e15993f49fb7d1982b209f4f1b394a3b7463b653691330' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21370.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
