ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22059.2/dotnet-runtime-7.0.0-alpha.1.22059.2-linux-x64.tar.gz \
    && dotnet_sha512='d63bf51e9593a1e33fb2add7d184245a4dcf776c14eb8ca4f3a77925d26c7046887f28a34a0d17a36a571afdef506864d3025136155f55497cfd4fb79b34942f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22059.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
