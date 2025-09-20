#!/usr/bin/env pwsh

# Test script for Findings Service GraphQL API
Write-Host "Testing Findings Service GraphQL API on http://localhost:5218/graphql" -ForegroundColor Green

# Test 1: Get all findings
$findingsQuery = @"
{
  "query": "{ findings { id title description referenceNumber identifiedDate severity priority category { name code } status { name code color } } }"
}
"@

Write-Host "`nTest 1: Get all findings" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5218/graphql" -Method Post -Body $findingsQuery -ContentType "application/json"
    Write-Host "✅ Success: Found $($response.data.findings.Count) findings" -ForegroundColor Green
    $response.data.findings | ForEach-Object { Write-Host "  - $($_.title) [$($_.referenceNumber)]" }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Get finding categories
$categoriesQuery = @"
{
  "query": "{ findingCategories { id name code description isActive } }"
}
"@

Write-Host "`nTest 2: Get finding categories" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5218/graphql" -Method Post -Body $categoriesQuery -ContentType "application/json"
    Write-Host "✅ Success: Found $($response.data.findingCategories.Count) categories" -ForegroundColor Green
    $response.data.findingCategories | ForEach-Object { Write-Host "  - $($_.name) [$($_.code)]" }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Get finding statuses
$statusesQuery = @"
{
  "query": "{ findingStatuses { id name code color displayOrder isFinalStatus isActive } }"
}
"@

Write-Host "`nTest 3: Get finding statuses" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5218/graphql" -Method Post -Body $statusesQuery -ContentType "application/json"
    Write-Host "✅ Success: Found $($response.data.findingStatuses.Count) statuses" -ForegroundColor Green
    $response.data.findingStatuses | ForEach-Object { Write-Host "  - $($_.name) [$($_.code)] Color: $($_.color)" }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🎉 Findings Service GraphQL Testing Complete!" -ForegroundColor Green
Write-Host "📍 Access GraphQL Playground: http://localhost:5218/graphql" -ForegroundColor Cyan