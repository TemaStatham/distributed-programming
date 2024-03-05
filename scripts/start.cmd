@echo off

cd ../Valuator
start cmd /k dotnet run --urls "http://0.0.0.0:5001"
start cmd /k dotnet run --urls "http://0.0.0.0:5002"

cd D:\programs\nginx-1.25.4\nginx-1.25.4
start cmd /k nginx.exe -c ..\..\..\..\programs\nginx-1.25.4\nginx-1.25.4\conf\nginx.conf