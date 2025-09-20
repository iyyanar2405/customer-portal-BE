# PowerShell script to organize stored procedures into modular folder structure
# This script will create folders for each module and move corresponding SP files

$storedProcedurePath = "C:\POC\customer-portal-BE\Stored-procedure"

# Define the module structure and corresponding stored procedures
$modules = @{
    "Actions" = @(
        "Sp_ActionFilterCategories.sql",
        "Sp_ActionFilterCompanies.sql", 
        "Sp_ActionFilterServices.sql",
        "Sp_ActionFilterSites.sql",
        "Sp_GetActions.sql",
        "Sp_InsertAction.sql"
    )
    "Audits" = @(
        "Sp_GetAuditDaysByMonthAndService.sql",
        "Sp_GetAuditDaysByService.sql",
        "Sp_GetAuditDaysGrid.sql",
        "Sp_GetAuditDetails.sql",
        "Sp_GetAuditFindingList.sql",
        "Sp_GetAuditSchedules.sql",
        "Sp_GetAuditSites.sql",
        "Sp_GetSubAudits.sql",
        "Sp_GetScheduleCalendarInvite.sql"
    )
    "Certificates" = @(
        "Sp_GetCertificateDetails.sql",
        "Sp_GetCertificateList.sql",
        "Sp_GetCertificateSites.sql"
    )
    "Contracts" = @(
        "Sp_GetContractList.sql"
    )
    "Findings" = @(
        "Sp_GetFindingDetails.sql",
        "Sp_GetFindingList.sql",
        "Sp_GetFindingResponseHistory.sql",
        "Sp_GetFindingsByClauseList.sql",
        "Sp_GetFindingsTrendsGraphData.sql",
        "Sp_GetFindingTrendsList.sql",
        "Sp_GetLatestFindingResponse.sql",
        "Sp_GetManageFindingDetails.sql",
        "Sp_PostLatestFindingResponse.sql"
    )
    "Financial" = @(
        "Sp_DownloadInvoice.sql",
        "Sp_GetInvoiceList.sql",
        "Sp_GetOverviewFinancialStatus.sql",
        "Sp_UpdatePlannedPaymentDate.sql"
    )
    "Notifications" = @(
        "Sp_NotificationFilterCategories.sql",
        "Sp_NotificationFilterCompanies.sql",
        "Sp_NotificationFilterServices.sql",
        "Sp_NotificationFilterSites.sql",
        "Sp_Notifications.sql"
    )
    "Overview" = @(
        "Sp_GetOverviewCardData.sql",
        "Sp_GetOverviewCompanyServiceSiteFilter.sql"
    )
    "Settings" = @(
        "Sp_GetPreferences.sql",
        "Sp_GetSettingsAdminList.sql",
        "Sp_GetSettingsCompanyDetails.sql",
        "Sp_GetSettingsMemberList.sql"
    )
    "Users" = @(
        "Sp_GetUserProfile.sql",
        "Sp_GetUserValidation.sql"
    )
    "Widgets" = @(
        "Sp_GetWidgetForTrainingStatus.sql",
        "Sp_GetWidgetForUpcomingAudit.sql"
    )
    "Master" = @(
        "Sp_GetCountryList.sql",
        "Sp_GetMasterSiteList.sql",
        "Sp_GetServiceList.sql"
    )
}

Write-Host "=== Organizing Stored Procedures by Module ===" -ForegroundColor Cyan

# Create module folders and move files
foreach ($module in $modules.Keys) {
    $modulePath = Join-Path $storedProcedurePath $module
    
    # Create module folder if it doesn't exist
    if (-not (Test-Path $modulePath)) {
        New-Item -ItemType Directory -Path $modulePath -Force | Out-Null
        Write-Host "Created folder: $module" -ForegroundColor Green
    } else {
        Write-Host "Folder already exists: $module" -ForegroundColor Yellow
    }
    
    # Move stored procedures to module folder
    foreach ($spFile in $modules[$module]) {
        $sourcePath = Join-Path $storedProcedurePath $spFile
        $destinationPath = Join-Path $modulePath $spFile
        
        if (Test-Path $sourcePath) {
            Move-Item -Path $sourcePath -Destination $destinationPath -Force
            Write-Host "  Moved: $spFile -> $module/" -ForegroundColor Green
        } else {
            Write-Host "  Warning: File not found - $spFile" -ForegroundColor Red
        }
    }
    
    Write-Host ""
}

Write-Host "=== Module Organization Summary ===" -ForegroundColor Cyan

# Display final folder structure
foreach ($module in $modules.Keys | Sort-Object) {
    $modulePath = Join-Path $storedProcedurePath $module
    $fileCount = (Get-ChildItem -Path $modulePath -Filter "*.sql" -ErrorAction SilentlyContinue).Count
    Write-Host "$module/: $fileCount stored procedures" -ForegroundColor White
    
    # List files in each module
    Get-ChildItem -Path $modulePath -Filter "*.sql" -ErrorAction SilentlyContinue | ForEach-Object {
        Write-Host "  - $($_.Name)" -ForegroundColor Gray
    }
    Write-Host ""
}

Write-Host "Stored procedure organization completed!" -ForegroundColor Cyan