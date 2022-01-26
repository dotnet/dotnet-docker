ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.22-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.22 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='32332a09a2f05e56ca66fff2a27e988dc35dc12dcc312b62c99c2a7b2532fb02bab0fa7f49f73938999488c0463a0c79c151b020e01104b8e9cfab80a877b5f3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
