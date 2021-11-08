ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.12/dotnet-runtime-5.0.12-linux-x64.tar.gz \
    && dotnet_sha512='32b5f86db3b1d4c21e3cf616d22f0e4a7374385dac0cf03cdebf3520dcf846460d9677ec1829a180920740a0237d64f6eaa2421d036a67f4fe9fb15d4f6b1db9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.12-bullseye-slim-amd64

ENV DOTNET_VERSION=5.0.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
