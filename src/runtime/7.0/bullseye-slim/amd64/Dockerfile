ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22066.4/dotnet-runtime-7.0.0-alpha.1.22066.4-linux-x64.tar.gz \
    && dotnet_sha512='54035da58611ad409fa12026aabc3b1eb1ace645e2f4409762968d2197d6d288856425d0433eabc564a69906a08fe9dbab8ce730d5f7d649c9b09ddeb94a2477' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22066.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
