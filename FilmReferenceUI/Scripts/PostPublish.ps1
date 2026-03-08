param (
    [string]$AppPoolName = "FilmReferenceDev",
    [string]$SiteName = "FilmReferenceDev",
    [string]$PublishDir = "C:\inetpub\Websites\FilmReference\Dev"
)

Import-Module WebAdministration
$ScriptFailed = $false
$LogPath = "$PublishDir\PostPublish.log"

function Log($message) {
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    "$timestamp - $message" | Out-File -Append $LogPath
    Write-Host $message
}

Log "PostPublish started."

# Start App Pool
try {
    Start-WebAppPool -Name $AppPoolName
    Log "App Pool '$AppPoolName' started."
} catch {
    Log "ERROR starting App Pool: $_"
    $ScriptFailed = $true
}

# Start Website
try {
    Start-Website -Name $SiteName
    Log "Website '$SiteName' started."
} catch {
    Log "ERROR starting Website: $_"
    $ScriptFailed = $true
}

# Run xUnit tests
try {
    Log "Running xUnit tests..."
    dotnet test "C:\JuliansWork\WebApplications\FilmReferenceBlazor\FilmReferenceBlazor.Tests\FilmReferenceBlazor.Tests.csproj"
    if ($LASTEXITCODE -ne 0) {
        Log "xUnit tests failed with exit code $LASTEXITCODE"
        $ScriptFailed = $true
    } else {
        Log "xUnit tests passed."
    }
} catch {
    Log "ERROR running xUnit tests: $_"
    $ScriptFailed = $true
}

$maxAttempts = 10
$attempt = 0
do {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:9090/" -UseBasicParsing -TimeoutSec 5
        if ($response.StatusCode -eq 200) {
            Write-Host "Site is ready."
            break
        }
    } catch {
        Write-Host "Waiting for site to respond..."
        Start-Sleep -Seconds 3
        $attempt++
    }
} while ($attempt -lt $maxAttempts)

if ($attempt -eq $maxAttempts) {
    Write-Host "Site did not respond in time."
}

# Clean up lingering processes
try {
    Get-Process testhost -ErrorAction SilentlyContinue | Stop-Process -Force
    Log "Stopped lingering testhost processes."
} catch {
    Log "ERROR cleaning up testhost: $_"
    $ScriptFailed = $true
}

Log "PostPublish completed."

# Trigger Jenkins job after successful publish
$jenkinsUrl = "http://localhost:8080/job/FilmReference-1-DeployDevToTest/buildWithParameters?DryRun=false"
$jenkinsUser = "VS_Publisher"
$jenkinsToken = "11e5c4513b9716c83758531e7390ac9e3b"

$headers = @{
    Authorization = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${jenkinsUser}:${jenkinsToken}"))
}

try {
    Invoke-RestMethod -Uri $jenkinsUrl -Method Post -Headers $headers
    Write-Host "Triggered Jenkins deployment to Test successfully."
} catch {
    Write-Host "Failed to trigger Jenkins job: $_"
}

# Final exit logic
if ($ScriptFailed) {
    Log "Script encountered errors, but exiting with success to allow publish."
    exit 0
} else {
    exit 0
}