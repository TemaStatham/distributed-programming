taskkill /IM dotnet.exe /F
cd D:\programs\nginx-1.25.4\nginx-1.25.4
start cmd ./nginx -s stop
taskkill /IM nginx.exe /F