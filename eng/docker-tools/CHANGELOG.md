# Docker Tools / ImageBuilder Changelog

All breaking changes and new features in `eng/docker-tools` will be documented in this file.

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
