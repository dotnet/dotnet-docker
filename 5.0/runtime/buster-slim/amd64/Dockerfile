ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM buildpack-deps:buster-curl as installer

# Retrieve .NET Core
RUN dotnet_version=5.0.0-alpha.1.20103.10 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='abb62ddeca05441d75e0feeafb8b02c20d60664297b628f8b2fb250eb703c07b09be0ee61c24db7651daf6d8cbc900b019ddc2102a987910da91e0ef43e1086f' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-buster-slim

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
