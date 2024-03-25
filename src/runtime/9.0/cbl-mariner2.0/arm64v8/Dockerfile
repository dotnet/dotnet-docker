ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=9.0.0-preview.4.24173.6 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='0a7027c89a8dcfe65f62e01bfffad7286ef2ac8ad69add7136cf7586f95909ea8186850b10cf7e837154a7157252622c452f64236f0dfe7e81487c39d5cdfa59' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=9.0.0-preview.4.24173.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
