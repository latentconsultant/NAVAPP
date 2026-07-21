# Sharing script for Local Network
# This script identifies your IP address and provides the URL for other computers to connect

$port = 8080 # Default port for Docker and Production

# Get the local IPv4 address
$ip = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.InterfaceAlias -notlike "*Loopback*" -and $_.IPv4Address -notlike "169.254.*" } | Select-Object -First 1).IPv4Address

if (-not $ip) {
    Write-Host "Could not automatically detect your local IP address." -ForegroundColor Red
    Write-Host "Please run 'ipconfig' manually to find your IPv4 Address." -ForegroundColor Yellow
} else {
    Write-Host "--- Local Network Sharing ---" -ForegroundColor Cyan
    Write-Host "Your Host IP: $ip" -ForegroundColor Green
    Write-Host ""
    Write-Host "To allow others on your network to connect, give them this link:" -ForegroundColor White
    Write-Host "http://$($ip):$port" -ForegroundColor Green -NoNewline
    Write-Host " (or http://$($ip):$port/swagger for API)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "IMPORTANT:" -ForegroundColor Yellow
    Write-Host "1. Ensure the application is RUNNING (e.g., via 'docker-compose up')."
    Write-Host "2. Ensure Port $port is OPEN in your Windows Firewall."
    Write-Host ""
    Write-Host "Would you like to open the Windows Firewall settings now? (y/n)" -NoNewline
    $input = Read-Host
    if ($input -eq "y") {
        Start-Process "control" "firewall.cpl"
    }
}
