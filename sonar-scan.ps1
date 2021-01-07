param (
    [string]$sonarProjectName = "",
    [string]$sonarProjectKey = "",
    [string]$sonarLogin = ""
 )

Write-Host "Scanning..."

dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:$sonarProjectKey /n:$sonarProjectName /d:sonar.login=$sonarLogin /d:sonar.verbose="true" /d:sonar.qualitygate.wait="true" /d:sonar.cs.opencover.reportsPaths='"coverage/**/coverage.opencover.xml"' /d:sonar.coverage.exclusions='"tests/**/**"'
dotnet build . --configuration FAKE_XRM_EASY_9 --framework net462
dotnet test . --configuration FAKE_XRM_EASY_9 --framework net462 --verbosity normal --collect:"XPlat code coverage" --settings tests/.runsettings --results-directory ./coverage
dotnet sonarscanner end /d:sonar.login=$sonarLogin

Write-Host "Scanning Complete :)" -ForegroundColor Green