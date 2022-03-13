param (
    [string]$targetFrameworks = "netcoreapp3.1",
    [string]$packTests = ""
 )


function Build-Config($configuration)
{
    Write-Host "Building '$($configuration)' configuration..." -ForegroundColor Yellow
    ./build.ps1 -targetFrameworks $targetFrameworks -configuration $configuration
}

Build-Config("FAKE_XRM_EASY")
Build-Config("FAKE_XRM_EASY_2013")
Build-Config("FAKE_XRM_EASY_2015")
Build-Config("FAKE_XRM_EASY_2016")
Build-Config("FAKE_XRM_EASY_365")
Build-Config("FAKE_XRM_EASY_9")

Write-Host "Done." -ForegroundColor Green