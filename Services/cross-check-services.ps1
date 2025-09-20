# Service Cross-Check Report
# Comparing Documentation vs Actual Implementation

Write-Host "=== Customer Portal Services Cross-Check ===" -ForegroundColor Green

# Services from documentation
$documentedServices = @(
    @{Name="Master Service"; DocPort=5067; ExpectedPath="CustomerPortal.MasterService"},
    @{Name="Users Service"; DocPort=5004; ExpectedPath="CustomerPortal.UsersService"},
    @{Name="Audits Service"; DocPort=5002; ExpectedPath="CustomerPortal.AuditsService"},
    @{Name="Actions Service"; DocPort=5001; ExpectedPath="CustomerPortal.ActionsService"},
    @{Name="Certificates Service"; DocPort=5005; ExpectedPath="CustomerPortal.CertificatesService"},
    @{Name="Contracts Service"; DocPort=6006; ExpectedPath="CustomerPortal.ContractsService"},
    @{Name="Financial Service"; DocPort=5007; ExpectedPath="CustomerPortal.FinancialService"},
    @{Name="Findings Service"; DocPort=5008; ExpectedPath="CustomerPortal.FindingsService"},
    @{Name="Notifications Service"; DocPort=5009; ExpectedPath="CustomerPortal.NotificationsService"},
    @{Name="Settings Service"; DocPort=5010; ExpectedPath="CustomerPortal.SettingsService"},
    @{Name="Overview Service"; DocPort=5011; ExpectedPath="CustomerPortal.OverviewService"},
    @{Name="Widgets Service"; DocPort=5012; ExpectedPath="CustomerPortal.WidgetsService"},
    @{Name="API Gateway"; DocPort=5000; ExpectedPath="CustomerPortal.Gateway"}
)

$servicesPath = "C:\POC\local\customer-portalBE\customer-portal-BE\Services"
$results = @()

foreach ($service in $documentedServices) {
    $servicePath = Join-Path $servicesPath $service.ExpectedPath
    $launchSettingsPath = Join-Path $servicePath "Properties\launchSettings.json"
    
    $result = [PSCustomObject]@{
        ServiceName = $service.Name
        DocumentedPort = $service.DocPort
        ActualPort = "N/A"
        ProjectExists = Test-Path $servicePath
        LaunchSettingsExists = Test-Path $launchSettingsPath
        HasGraphQL = $false
        HasEntities = $false
        Status = "Unknown"
    }
    
    if ($result.ProjectExists) {
        Write-Host "✅ Found: $($service.Name)" -ForegroundColor Green
        
        # Check launch settings for actual port
        if ($result.LaunchSettingsExists) {
            try {
                $launchSettings = Get-Content $launchSettingsPath | ConvertFrom-Json
                $httpProfile = $launchSettings.profiles.http
                if ($httpProfile -and $httpProfile.applicationUrl) {
                    $actualPort = ($httpProfile.applicationUrl -split ":")[-1]
                    $result.ActualPort = $actualPort
                    
                    if ($actualPort -eq $service.DocPort) {
                        Write-Host "  ✅ Port matches documentation: $actualPort" -ForegroundColor Green
                    } else {
                        Write-Host "  ⚠️  Port mismatch - Doc: $($service.DocPort), Actual: $actualPort" -ForegroundColor Yellow
                    }
                }
            } catch {
                Write-Host "  ❌ Error reading launch settings" -ForegroundColor Red
            }
        }
        
        # Check for GraphQL implementation
        $graphqlPath = Join-Path $servicePath "GraphQL"
        if (Test-Path $graphqlPath) {
            $result.HasGraphQL = $true
            Write-Host "  ✅ Has GraphQL implementation" -ForegroundColor Green
        } else {
            Write-Host "  ⚠️  No GraphQL implementation found" -ForegroundColor Yellow
        }
        
        # Check for Entities
        $entitiesPath = Join-Path $servicePath "Entities"
        if (Test-Path $entitiesPath) {
            $result.HasEntities = $true
            Write-Host "  ✅ Has Entities" -ForegroundColor Green
        } else {
            Write-Host "  ⚠️  No Entities found" -ForegroundColor Yellow
        }
        
        # Check Program.cs for implementation
        $programPath = Join-Path $servicePath "Program.cs"
        if (Test-Path $programPath) {
            $programContent = Get-Content $programPath -Raw
            if ($programContent -match "GraphQL|AddGraphQLServer") {
                Write-Host "  ✅ GraphQL configured in Program.cs" -ForegroundColor Green
                $result.Status = "Implemented"
            } elseif ($programContent -match "AddControllers|MapControllers") {
                Write-Host "  ⚠️  Basic API structure only" -ForegroundColor Yellow
                $result.Status = "Basic Structure"
            } else {
                Write-Host "  ❌ Minimal implementation" -ForegroundColor Red
                $result.Status = "Minimal"
            }
        }
        
    } else {
        Write-Host "❌ Missing: $($service.Name)" -ForegroundColor Red
        $result.Status = "Missing"
    }
    
    $results += $result
    Write-Host ""
}

# Summary Report
Write-Host "=== SUMMARY REPORT ===" -ForegroundColor Cyan
Write-Host ""

$implemented = ($results | Where-Object {$_.Status -eq "Implemented"}).Count
$basicStructure = ($results | Where-Object {$_.Status -eq "Basic Structure"}).Count
$minimal = ($results | Where-Object {$_.Status -eq "Minimal"}).Count
$missing = ($results | Where-Object {$_.Status -eq "Missing"}).Count
$total = $results.Count

Write-Host "Total Services: $total" -ForegroundColor White
Write-Host "✅ Fully Implemented: $implemented" -ForegroundColor Green
Write-Host "⚠️  Basic Structure: $basicStructure" -ForegroundColor Yellow
Write-Host "⚠️  Minimal: $minimal" -ForegroundColor Yellow
Write-Host "❌ Missing: $missing" -ForegroundColor Red
Write-Host ""

# Port Mismatches
$portMismatches = $results | Where-Object {$_.DocumentedPort -ne $_.ActualPort -and $_.ActualPort -ne "N/A"}
if ($portMismatches.Count -gt 0) {
    Write-Host "=== PORT MISMATCHES ===" -ForegroundColor Red
    foreach ($mismatch in $portMismatches) {
        Write-Host "$($mismatch.ServiceName): Doc=$($mismatch.DocumentedPort), Actual=$($mismatch.ActualPort)" -ForegroundColor Yellow
    }
    Write-Host ""
}

# Services needing implementation
$needsImplementation = $results | Where-Object {$_.Status -in @("Missing", "Minimal", "Basic Structure")}
if ($needsImplementation.Count -gt 0) {
    Write-Host "=== SERVICES NEEDING WORK ===" -ForegroundColor Yellow
    foreach ($service in $needsImplementation) {
        Write-Host "- $($service.ServiceName): $($service.Status)" -ForegroundColor Yellow
    }
}

# Export results to CSV
$results | Export-Csv -Path ".\service-cross-check-results.csv" -NoTypeInformation
Write-Host ""
Write-Host "Detailed results exported to: service-cross-check-results.csv" -ForegroundColor Cyan