# Alpine Floating Tags Updated to Alpine {{ NewVersion }}

In {{ PublishDate | date: "%B %Y" }}, [Alpine {{ NewVersion }} container images were published]({{ PublishDiscussionUrl }}). For today's {{ ReleaseDate | date: "%B %Y" }} .NET release, all Alpine floating tags now point to Alpine {{ NewVersion }} instead of Alpine {{ OldVersion }} according to our [tagging policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md).

Per the [.NET Docker platform support policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md#linux), Alpine {{ OldVersion }} images will no longer be maintained starting on {{ EolDate | date: "%B %-d, %Y" }} (3 months after Alpine {{ NewVersion }} images were released).

## Details

Please review the [Alpine {{ NewVersion }} changelog](https://alpinelinux.org/posts/Alpine-{{ NewVersion }}.0-released.html) for more details on changes that were made in this version of Alpine.

The affected floating tags use this naming pattern:

* `<version>-alpine` (e.g. `{{ DotnetExampleVersion }}-alpine`)
* `<version>-alpine-<variant>` (e.g. `{{ DotnetExampleVersion }}-alpine-extra`)
* `<version>-alpine-<arch>` (e.g. `{{ DotnetExampleVersion }}-alpine-amd64`)
* `<version>-alpine-<variant>-<arch>` (e.g. `{{ DotnetExampleVersion }}-alpine-extra-amd64`)

The following image repos have been updated:

* dotnet/sdk - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/sdk/about)
* dotnet/aspnet - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/aspnet/about)
* dotnet/runtime - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/runtime/about)
* dotnet/runtime-deps - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/runtime-deps/about)
