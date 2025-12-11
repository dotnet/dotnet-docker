{{
    set commonArgs to [
        "top-header": ARGS["top-header"]
        "readme-host": ARGS["readme-host"]
    ] ^

    set insertReposListTemplate(template) to:{{
        return InsertTemplate("ReposProvider.md", union([ "template": template ], commonArgs))
    }} ^

    set isNightlyRepo to match(split(REPO, "/")[1], "nightly") ^
    set readmeRepoName to when(PARENT_REPO = "monitor", cat("monitor-", SHORT_REPO), SHORT_REPO)

}}{{InsertTemplate("Announcement.md", union(commonArgs, [ "trailing-line-break": "true" ]))}}{{
if !IS_PRODUCT_FAMILY:{{InsertTemplate("FeaturedTags.md", commonArgs)}}
}}{{if IS_PRODUCT_FAMILY:{{
    insertReposListTemplate("FeaturedRepos.md")}}
}}
{{InsertTemplate("About.md", commonArgs)}}

{{InsertTemplate("Use.md", commonArgs)}}{{if (find(REPO, "monitor") < 0 && find(REPO, "aspire") < 0):

{{InsertTemplate("About.variants.md", commonArgs)}}}}

{{insertReposListTemplate("RelatedRepos.md")}}
{{if !IS_PRODUCT_FAMILY:
{{ARGS["top-header"]}} Full Tag Listing
{{if ARGS["readme-host"] = "github":<!--End of generated tags-->
*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md). See the [full list of tags](https://mcr.microsoft.com/v2/{{REPO}}/tags/list) for all supported and unsupported tags.*
^elif ARGS["readme-host"] = "dockerhub":
View the current tags at the [Microsoft Artifact Registry portal](https://mcr.microsoft.com/artifact/mar/{{REPO}}/tags) or on [GitHub](https://github.com/dotnet/dotnet-docker/blob/{{if isNightlyRepo:nightly^else:main}}/README.{{readmeRepoName}}.md#full-tag-listing).
}}}}
{{InsertTemplate("Support.md", commonArgs)}}
