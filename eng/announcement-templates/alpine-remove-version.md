# .NET Alpine {{ RemovedVersion }} images no longer maintained

.NET container images based on Alpine {{ RemovedVersion }} will no longer be maintained starting today, {{ EolDate | date: "%Y-%m-%d" }}, per our policy. Alpine {{ ReplacementVersion }} images [were released on {{ ReplacementReleaseDate | date: "%Y-%m-%d" }}]({{ ReplacementDiscussionUrl }}).

For more information, see our policies:

- [Supported platforms policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-platforms.md)
- [Supported tags policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md#majorminor-net-version-alpine).

## Details

Image tags containing `alpine{{ RemovedVersion }}` will no longer be updated. We will not delete existing {{ RemovedVersion }} images or tags, but they now contain unsupported builds of Alpine and .NET.

If you are using one of the Alpine {{ RemovedVersion }} tags such as `{{ DotnetExampleVersion1 }}-alpine{{ RemovedVersion }}` or `{{ DotnetExampleVersion2 }}-alpine{{ RemovedVersion }}`, we recommend that you upgrade to Alpine {{ ReplacementVersion }}{% if NewestVersion != "" %} or Alpine {{ NewestVersion }}{% endif %}. Alternatively, you can use a floating tag for Alpine such as `{{ DotnetExampleVersion1 }}-alpine` or `{{ DotnetExampleVersion2 }}-alpine` which updates automatically according to the [supported tags policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md#majorminor-net-version-alpine).
