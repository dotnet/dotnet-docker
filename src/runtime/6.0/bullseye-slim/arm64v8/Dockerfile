ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21419.17/dotnet-runtime-6.0.0-rc.1.21419.17-linux-arm64.tar.gz \
    && dotnet_sha512='617157c33e1a5dcf0d53ea7138fe4878aa17208c159654f6d0adb2d7bfc2e9ccd5084b700c827296c1e11629b75bdad65cd805346c6d9155ba4223c37d54eae5' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21419.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
