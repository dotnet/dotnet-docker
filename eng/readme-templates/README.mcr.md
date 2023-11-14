{{
  set commonArgs to [ "top-header": "##", "readme-host": "mar" ]
}}{{InsertTemplate("About.md", commonArgs)}}

{{InsertTemplate("FeaturedTags.md", commonArgs)}}

{{InsertTemplate("RelatedRepos.md", commonArgs)}}

{{InsertTemplate("Use.md", commonArgs)}}{{if find(REPO, "monitor") < 0:

{{InsertTemplate("About.variants.md", commonArgs)}}}}

{{InsertTemplate("Support.md", commonArgs)}}
