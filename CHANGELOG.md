## [2.3.0]

### Added

- It is now possible to use XrmRealContext to execute a code activity for integration test purposes - https://github.com/DynamicsValue/fake-xrm-easy/issues/35
 
## [2.2.0]

### Changed

- Fix Sonar Quality Gate settings: DynamicsValue/fake-xrm-easy#28

## [2.1.1]

### Changed

- Made CRM SDK v8.2 dependencies less specific - DynamicsValue/fake-xrm-easy#21
- Updated build script to also include the major version in the Title property of the generated .nuspec file - DynamicsValue/fake-xrm-easy#41
- Limit FakeItEasy package dependency to v6.x versions - DynamicsValue/fake-xrm-easy#37

## [2.1.0]

### Changed

- Bump Microsoft.CrmSdk.CoreAssemblies to version 9.0.2.27 to support plugin telemetry - DynamicsValue/fake-xrm-easy#24
- Update package reference ranges to use 2.x versions only.
- Populate IServiceEndpointNotificationService into the execution context of the code activity from a fake workflow context DynamicsValue/fake-xrm-easy#18

## [2.0.1-rc1] - Initial release