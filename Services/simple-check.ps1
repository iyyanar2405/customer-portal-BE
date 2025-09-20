# Manual Service Port Check
Write-Host "=== Manual Service Check ===" -ForegroundColor Green

$services = @(
    "CustomerPortal.ActionsService",
    "CustomerPortal.AuditsService", 
    "CustomerPortal.CertificatesService",
    "CustomerPortal.ContractsService",
    "CustomerPortal.FinancialService",
    "CustomerPortal.FindingsService",
    "CustomerPortal.Gateway",
    "CustomerPortal.MasterService",
    "CustomerPortal.NotificationsService",
    "CustomerPortal.OverviewService",
    "CustomerPortal.SettingsService",
    "CustomerPortal.UsersService",
    "CustomerPortal.WidgetsService"
)

foreach ($service in $services) {
    $launchSettings = "C:\POC\local\customer-portalBE\customer-portal-BE\Services\$service\Properties\launchSettings.json"
    
    if (Test-Path $launchSettings) {
        $content = Get-Content $launchSettings | ConvertFrom-Json
        $port = ($content.profiles.http.applicationUrl -split ":")[-1]
        Write-Host "$service : Port $port" -ForegroundColor Cyan
        
        # Check for GraphQL
        $programCs = "C:\POC\local\customer-portalBE\customer-portal-BE\Services\$service\Program.cs"
        if (Test-Path $programCs) {
            $program = Get-Content $programCs -Raw
            if ($program -match "GraphQL") {
                Write-Host "  ✅ Has GraphQL" -ForegroundColor Green
            } else {
                Write-Host "  ⚠️  No GraphQL" -ForegroundColor Yellow
            }
        }
    } else {
        Write-Host "$service : No launch settings" -ForegroundColor Red
    }
}