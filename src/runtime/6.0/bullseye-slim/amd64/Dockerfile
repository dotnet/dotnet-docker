ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21430.11/dotnet-runtime-6.0.0-rc.1.21430.11-linux-x64.tar.gz \
    && dotnet_sha512='b3c7cc345f43106851c96164532cadbd66d67dea17ee739731ef298a9aa7e4b60588a28b2062a210aaf6bdfe5c06cdfecced06a9b859009b4d8e11ec1886c57d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21430.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
