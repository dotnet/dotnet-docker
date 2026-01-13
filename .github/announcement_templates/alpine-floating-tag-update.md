# Alpine Floating Tags Updated to Alpine {{ new_version }}

In {{ publish_date | date: "%B %Y" }}, [Alpine {{ new_version }} container images were published](https://github.com/dotnet/dotnet-docker/discussions/{{ publish_discussion_number }}). For today's {{ release_date | date: "%B %Y" }} .NET release, all Alpine floating tags now point to Alpine {{ new_version }} instead of Alpine {{ old_version }} according to our [tagging policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md).

Per the [.NET Docker platform support policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md#linux), Alpine {{ old_version }} images will no longer be maintained starting on {{ eol_date | date: "%B %-d, %Y" }} (3 months after Alpine {{ new_version }} images were released).

## Details

Please review the [Alpine {{ new_version }} changelog](https://alpinelinux.org/posts/Alpine-{{ new_version }}.0-released.html) for more details on changes that were made in this version of Alpine.

The affected floating tags use this naming pattern:

* `<version>-alpine` (e.g. `{{ dotnet_example_version }}-alpine`)
* `<version>-alpine-<variant>` (e.g. `{{ dotnet_example_version }}-alpine-extra`)
* `<version>-alpine-<arch>` (e.g. `{{ dotnet_example_version }}-alpine-amd64`)
* `<version>-alpine-<variant>-<arch>` (e.g. `{{ dotnet_example_version }}-alpine-extra-amd64`)

The following image repos have been updated:

* dotnet/sdk - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/sdk/about)
* dotnet/aspnet - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/aspnet/about)
* dotnet/runtime - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/runtime/about)
* dotnet/runtime-deps - [Microsoft Artifact Registry](https://mcr.microsoft.com/product/dotnet/runtime-deps/about)
