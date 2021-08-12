ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.9/dotnet-runtime-5.0.9-linux-arm64.tar.gz \
    && dotnet_sha512='00f33e9c7f6ab653872952a3bf97bd51f8c764abbcb472218d722f6d5593a80b94a15c1a6fdefd0a4f8ed64874754903c3ab40e1761cff93bcaff3b5da82c47b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.9-buster-slim-arm64v8

ENV DOTNET_VERSION=5.0.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
