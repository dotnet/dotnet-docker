# Enabling (or disabling) globalization functionality

.NET includes various [globalization](https://learn.microsoft.com/dotnet/core/extensions/globalization-and-localization) capabilities, including support for processing natural language text, calendars, currency, and time zones. The .NET implementations for these capabilities comes primarily from operating system libraries, such as [International Components for Unicode (ICU)](https://icu.unicode.org/) and [tzdata](https://wikipedia.org/wiki/Tz_database). In some cases, these libraries and their accompanying databases are always available and in other cases it is desired that they be absent because they may be considered prohibitively large.

The .NET team has various policies for making these libraries available in containers and for configuring the .NET product to use them (or not).

## ICU

In many scenarios, globalization support with ICU is required, for example, to correctly sort a list of strings (particularly characters outside the [ASCII](https://en.wikipedia.org/wiki/ASCII) range). Other applications may not be oriented around natural language or other global concepts, and would prefer to optimize for container size. .NET container images have been curated to offer options for both of these scenarios.

.NET container images that include ICU:

- Alpine `sdk` images
- Debian images
- Ubuntu images

.NET container images that do not include ICU:

- Alpine `aspnet`, `monitor`, `runtime`, `runtime-deps` images
- Ubuntu chiseled images

Images that do not include ICU enable [Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/main/docs/design/features/globalization-invariant-mode.md), which provides more basic globalization behaviors in absence of using ICU.

Some users want to add ICU to one of the image types that doesn't include it. It is counter-productive to remove ICU from an image that already includes it.

### Alpine images

ICU can be added to a .NET Alpine image by adding the following instructions to the final stage within a `Dockerfile`, as demonstrated in [Dockerfile.alpine-icu](aspnetapp/Dockerfile.alpine-icu). This Dockerfile fragment adds and configures ICU and disables Globalization invariant mode.

```Dockerfile
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache \
    icu-data-full \
    icu-libs
```

### Ubuntu chiseled images

ICU can be added to an Ubuntu chiseled image, as demonstrated by https://github.com/ubuntu-rocks/dotnet/issues/21.

## Tzdata

Tzdata provides data about time zones. It is needed in applications that deal with time.

.NET container images that include tzdata:

- Debian images

.NET container images that do not include ICU:

- Alpine images
- Ubuntu images
- Ubuntu chiseled images

If you want tzdata, you can install it. There is no similar "invariant" mode to consider.

You can run the following test to determine if `tzdata` is installed correctly.

```bash
~ # cat /etc/timezone
America/Los_Angeles
~ # date
Sat Feb 11 18:40:16 PST 2023
~ #
```

If those values seem correct, then they are. If no timezone is provided, then system defaults to UTC.

### Alpine

You can install data by `apk add tzdata`. If you want to also set the timezone, use the following approach (with the appropriate timezone).

```bash
TZ="America/Los_Angeles" && ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone && apk add tzdata
```

### Ubuntu

You can install `tzdata` but need to be aware that it prompts. The [following approach](https://dev.to/setevoy/docker-configure-tzdata-and-timezone-during-build-20bk) resolves that.

```bash
apt update && DEBIAN_FRONTEND=noninteractive && TZ="America/Los_Angeles" && ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone && apt install -y tzdata
```

You may need to go through the interactive experience once to find the appropriate `TZ` string. It will be recorded in `/etc/timezone`.
