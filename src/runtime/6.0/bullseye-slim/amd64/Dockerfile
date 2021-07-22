ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21371.11/dotnet-runtime-6.0.0-preview.7.21371.11-linux-x64.tar.gz \
    && dotnet_sha512='cdc149a42ebaf600c9594a4b92f43f701c5a447f697e85eb05ad881a58cf5a285bc339fb6f6793562375e07108a6e39404cadc5750b2f6910d4fd75e95c89457' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-preview.7.21371.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
