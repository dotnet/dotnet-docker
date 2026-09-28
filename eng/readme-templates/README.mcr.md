{{
  set commonArgs to [ "top-header": "##", "readme-host": "mar" ] ^
  set isAspireDashboard to SHORT_REPO = "aspire-dashboard" || (split(REPO, "/")[0] = "aspire" && SHORT_REPO = "dashboard")
}}{{InsertTemplate("About.md", commonArgs)}}

{{InsertTemplate("FeaturedTags.md", commonArgs)}}

{{if !isAspireDashboard:{{InsertTemplate("ReposProvider.md", union([ "template": "RelatedRepos.md" ], commonArgs))}}

}}{{InsertTemplate("Use.md", commonArgs)}}{{if (find(REPO, "monitor") < 0 && find(REPO, "aspire") < 0 && find(REPO, "yarp") < 0):

{{InsertTemplate("About.variants.md", commonArgs)}}}}

{{InsertTemplate("Support.md", commonArgs)}}
