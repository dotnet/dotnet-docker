ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Create standard image
FROM $REPO:5.0-alpine3.11 AS StandardImage

# Install .NET Core

RUN dotnet_version=5.0.0-preview.2.20160.6 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='f8dcf9f498feffbca4b0649086cffd298cd98189b2950c32dd52b07179b7f33e1acfe14ae59a266502a3518089ca55f224e007628d16194f94be7dc419c1b2b4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.2.20167.3 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='029ed32f8a7dfdb258438177b7fd40c187830db35b3d973dec296d8948f99d30a74269c45d31876c9b4533b8d7b6af65dea319299bd9a7081c6881cf026274b4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz

RUN dotnet --list-runtimes

# CROSSGEN the binaries from the standard image
FROM $REPO:5.0-alpine3.11 AS SdkImage

ENV \
    # Unset the value from the base image
    ASPNETCORE_URLS= \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetCoreSDK-Alpine-3.10

# Add dependencies for disabling invariant mode (set in base image)
RUN apk add --no-cache icu-libs

# Install .NET Core SDK
RUN dotnet_sdk_version=5.0.100-preview.2.20175.2 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='07b2e91410306297d7d8ccd84aa00be06181efb8cb3cc4fab57cd370aca65409ba59c87dba70d2b46e1dd05c0597c2f0c8f38cde35032910febb08f51b65731a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.0.0 \
    && wget -O PowerShell.Linux.Alpine.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && powershell_sha512='afca5d8553d612e36d04597de14cdba9731442d567d25fb9b0f1451116f299f773b4f49b5be7d4d89e3e874eb43f8c062ae70c2ed1d620244a2a52ba443cf4cb' \
    && echo "$powershell_sha512  PowerShell.Linux.Alpine.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.Alpine \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && chmod 755 /usr/share/powershell/pwsh \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm \
    # Add ncurses-terminfo-base to resolve psreadline dependency
    && apk add --no-cache ncurses-terminfo-base

RUN dotnet --list-runtimes
RUN dotnet --list-sdks

RUN dotnet --list-runtimes | grep Microsoft.NETCore.App | cut -d ' ' -f 2 > runtime-version.txt
RUN dotnet --list-runtimes | grep Microsoft.AspNetCore.App | cut -d ' ' -f 2 > aspnet-version.txt

RUN dotnet new console -o /tmp
COPY nuget.config /tmp
RUN dotnet publish /tmp/tmp.csproj -p:PublishReadyToRun=true -p:PublishReadyToRunUseCrossgen2=true -r linux-musl-x64
COPY --from=StandardImage /usr/share/dotnet/shared/Microsoft.NETCore.App /usr/cpinput
COPY --from=StandardImage /usr/share/dotnet/shared/Microsoft.AspNetCore.App /usr/cpaspinput
RUN mkdir /usr/input
RUN cp /usr/cpinput/$(cat runtime-version.txt)/*.dll /usr/input
RUN cp /usr/cpaspinput/$(cat aspnet-version.txt)/*.dll /usr/input
RUN mkdir /usr/crossgen2
RUN ~/.nuget/packages/microsoft.netcore.app.crossgen2.linux-musl-x64/$(cat runtime-version.txt)/tools/crossgen2 --inputbubble -o /usr/crossgen2/sharedframework.dll --composite -O --Os /usr/input/*.dll

COPY --from=StandardImage /usr/share/dotnet /usr/output
RUN for b in /usr/output/shared/Microsoft.NETCore.App/$(cat runtime-version.txt)/*.dll; do a=/usr/crossgen2/`basename ${b}`; test -e ${a} && cp ${a} /usr/output/shared/Microsoft.NETCore.App/$(cat runtime-version.txt); done
RUN for b in /usr/output/shared/Microsoft.AspNetCore.App/$(cat aspnet-version.txt)/*.dll; do a=/usr/crossgen2/`basename ${b}`; test -e ${a} && cp ${a} /usr/output/shared/Microsoft.AspNetCore.App/$(cat aspnet-version.txt); done
RUN cp /usr/crossgen2/sharedframework.dll /usr/output/shared/Microsoft.NETCore.App/$(cat runtime-version.txt)

# Create optimized monolithic docker image
FROM $REPO:5.0-alpine3.11

COPY --from=SdkImage /usr/output /usr/share/dotnet
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
