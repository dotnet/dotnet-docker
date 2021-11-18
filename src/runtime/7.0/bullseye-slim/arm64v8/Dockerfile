ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21567.1/dotnet-runtime-7.0.0-alpha.1.21567.1-linux-arm64.tar.gz \
    && dotnet_sha512='dd2e94cc310c2546c3c694684a518860195ee35634921e46b0743d17601a500dcfc11e4ff51fd3858ed30db70b40f8f0f15a80a5244781649ae2cb8fcf4d8251' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.21567.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
