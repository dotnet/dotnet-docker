# Docker Tools / ImageBuilder Changelog

All breaking changes and new features in `eng/docker-tools` will be documented in this file.

---

## 2026-03-12: Service connection OIDC changes

- Pull request: [#2013](https://github.com/dotnet/docker-tools/pull/2013)
- Issue: [#2012](https://github.com/dotnet/docker-tools/issues/2012)

`setup-service-connections.yml` has been removed. Azure DevOps no longer
issues OIDC tokens for service connections referenced in a separate stage.
Service connections are now referenced per-job via
`reference-service-connections.yml`.

**How to update:**

- Remove any `serviceConnections` parameters passed to `1es-official.yml` or
  `1es-unofficial.yml` - they are no longer accepted.
- Remove any calls to `setup-service-connections.yml` from stage templates.
- Non-registry service connections (e.g., kusto, marStatus) should be passed
  via `additionalServiceConnections` to the job templates that need them.

---

## 2026-03-04: Pre-build validation gated by `preBuildTestScriptPath` variable

The `PreBuildValidation` job condition now checks the new `preBuildTestScriptPath` variable instead of `testScriptPath`.
This allows repos to independently control whether pre-build validation runs, without affecting functional tests.

The new variable defaults to `$(testScriptPath)`, so existing repos that have pre-build tests are not affected.
Repos that do not have pre-build tests can set `preBuildTestScriptPath` to `""` to skip the job entirely.

---

## 2026-02-19: Separate Registry Endpoints from Authentication

- Pull request: [#1945](https://github.com/dotnet/docker-tools/pull/1945)
- Issue: [#1914](https://github.com/dotnet/docker-tools/issues/1914)

Authentication details (`serviceConnection`, `resourceGroup`, `subscription`) have been moved from individual registry endpoints into a centralized `RegistryAuthentication` list.
This fixes an issue where ACR authentication could fail when multiple service connections existed for the same registry.

**Before:** Each registry endpoint embedded its own authentication:

```yaml
publishConfig:
  BuildRegistry:
    server: $(acr.server)
    repoPrefix: "my-prefix/"
    resourceGroup: $(resourceGroup)
    subscription: $(subscription)
    serviceConnection:
      name: $(serviceConnectionName)
      id: $(serviceConnection.id)
      clientId: $(serviceConnection.clientId)
      tenantId: $(tenant)
  PublishRegistry:
    server: $(acr.server)
    repoPrefix: "publish/"
    resourceGroup: $(resourceGroup)
    subscription: $(subscription)
    serviceConnection:
      name: $(publishServiceConnectionName)
      id: $(publishServiceConnection.id)
      clientId: $(publishServiceConnection.clientId)
      tenantId: $(tenant)
```

**After:** Registry endpoints only contain `server` and `repoPrefix`. Authentication is centralized:

```yaml
publishConfig:
  BuildRegistry:
    server: $(acr.server)
    repoPrefix: "my-prefix/"
  PublishRegistry:
    server: $(acr.server)
    repoPrefix: "publish/"
  RegistryAuthentication:
    - server: $(acr.server)
      resourceGroup: $(resourceGroup)
      subscription: $(subscription)
      serviceConnection:
        name: $(serviceConnectionName)
        id: $(serviceConnection.id)
        clientId: $(serviceConnection.clientId)
        tenantId: $(tenant)
```

How to update:
- Update any publishConfig parameters to match the new structure.
    - Multiple registries can share authentication. If two registries use the same ACR server, only one entry is needed in `RegistryAuthentication`.
    - The new structure should match [ImageBuilder's Configuration Model](https://github.com/dotnet/docker-tools/tree/a82572386854f15af441c50c6efa698a627e9f2b/src/ImageBuilder/Configuration).
- Update service connection setup (if using `setup-service-connections.yml`):
   - The template now supports looking up service connections from `publishConfig.RegistryAuthentication`
   - Use the new `usesRegistries` parameter to specify which registries need auth setup:
     ```yaml
     - template: eng/docker-tools/templates/stages/setup-service-connections.yml
       parameters:
         publishConfig: ${{ variables.publishConfig }}
         usesRegistries:
           - $(buildRegistry.server)
           - $(publishRegistry.server)
     ```
