ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.4.24173.6 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='205a01e26d4471ed497344ad3440ecf898c50cd3745e0ef0e9ef06f2dbdb5c0b8e32e90c3131e980aba22d217182b737d91e560ddb9dc402c117692e8558b50f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.4.24173.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
