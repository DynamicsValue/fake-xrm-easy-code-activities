param (
    [string]$sonarProjectName = "",
    [string]$sonarProjectKey = "",
    [string]$sonarLogin = "",
    [string]$sonarOrganisation = ""

 )

Write-Host "Scanning..."

dotnet tool install --global dotnet-sonarscanner
if(!($LASTEXITCODE -eq 0)) {
    throw "Error installing sonar scanner"
}

dotnet sonarscanner begin /key:"$($sonarProjectKey)" /name:"$($sonarProjectName)" /o:"$($sonarOrganisation)" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="$($sonarLogin)" /d:sonar.verbose="true" /d:sonar.qualitygate.wait="true" /d:sonar.cs.opencover.reportsPaths="coverage/**/coverage.opencover.xml" /d:sonar.coverage.exclusions="tests/**/**"
if(!($LASTEXITCODE -eq 0)) {
    throw "Error at sonar scan: begin"
}

dotnet build . --configuration FAKE_XRM_EASY_9 --framework net462
if(!($LASTEXITCODE -eq 0)) {
    throw "Error while building"
}
dotnet test . --configuration FAKE_XRM_EASY_9 --framework net462 --verbosity normal --collect:"XPlat code coverage" --settings tests/.runsettings --results-directory ./coverage
if(!($LASTEXITCODE -eq 0)) {
    throw "Error while running test step"
}

dotnet sonarscanner end /d:sonar.login="$($sonarLogin)"
if(!($LASTEXITCODE -eq 0)) {
    throw "Error at sonar scan: end"
}

Write-Host "Scanning Complete :)" -ForegroundColor Green