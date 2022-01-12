ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.1/dotnet-runtime-6.0.1-linux-arm64.tar.gz \
    && dotnet_sha512='10b8775d44088ddc1ae193ce41f456d1bbaad21f2dc993de75c2b076b6ffcb07bca446f52180c9a175715a1e47ad42a4862c43ef11b5cbc1023cb4da3c426146' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
