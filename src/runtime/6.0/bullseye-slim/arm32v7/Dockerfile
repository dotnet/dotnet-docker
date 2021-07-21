ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21370.18/dotnet-runtime-6.0.0-preview.7.21370.18-linux-arm.tar.gz \
    && dotnet_sha512='bd579ba9f118f91ed004bec3ace6e73050a54b562fac873e00409d08a38edcbef3317837ef9c65cb30597514349417fe979e48aec688e6347b06d1e1bdcd70e5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21370.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
