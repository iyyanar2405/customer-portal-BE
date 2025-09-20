@echo off
echo Starting Customer Portal Services...
echo.

echo ===================================
echo Starting Master Service (Port 5003)
echo ===================================
start "Master Service" cmd /k "cd /d C:\POC\local\customer-portalBE\customer-portal-BE && dotnet run --project Services\CustomerPortal.MasterService --urls http://localhost:5003"

timeout /t 3 /nobreak >nul

echo.
echo ===================================
echo Starting Users Service (Port 5004)
echo ===================================
start "Users Service" cmd /k "cd /d C:\POC\local\customer-portalBE\customer-portal-BE && dotnet run --project Services\CustomerPortal.UsersService --urls http://localhost:5004"

timeout /t 3 /nobreak >nul

echo.
echo ===================================
echo Starting Audits Service (Port 5002)
echo ===================================
start "Audits Service" cmd /k "cd /d C:\POC\local\customer-portalBE\customer-portal-BE && dotnet run --project Services\CustomerPortal.AuditsService --urls http://localhost:5002"

timeout /t 3 /nobreak >nul

echo.
echo All services are starting...
echo.
echo GraphQL Endpoints:
echo - Master Service: http://localhost:5003/graphql
echo - Users Service: http://localhost:5004/graphql  
echo - Audits Service: http://localhost:5002/graphql
echo.
echo Opening Master Service GraphQL Playground...
timeout /t 5 /nobreak >nul
start http://localhost:5003/graphql

pause