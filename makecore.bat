@echo off

if not exist dist mkdir dist
if not exist dist\Hprose mkdir dist\Hprose
if not exist dist\Hprose\Release mkdir dist\Hprose\Release
if not exist dist\Hprose\Debug mkdir dist\Hprose\Debug
if not exist dist\Hprose.Client mkdir dist\Hprose.Client
if not exist dist\Hprose.Client\Release mkdir dist\Hprose.Client\Release
if not exist dist\Hprose.Client\Debug mkdir dist\Hprose.Client\Debug

del dist\Hprose\Release\netcoreapp2.0 /s /q /f
del dist\Hprose\Debug\netcoreapp2.0 /s /q /f
del dist\Hprose.Client\Release\netcoreapp2.0 /s /q /f
del dist\Hprose.Client\Debug\netcoreapp2.0 /s /q /f

if not exist dist\Hprose\Release\netcoreapp2.0 mkdir dist\Hprose\Release\netcoreapp2.0
if not exist dist\Hprose\Debug\netcoreapp2.0 mkdir dist\Hprose\Debug\netcoreapp2.0
if not exist dist\Hprose.Client\Release\netcoreapp2.0 mkdir dist\Hprose.Client\Release\netcoreapp2.0
if not exist dist\Hprose.Client\Debug\netcoreapp2.0 mkdir dist\Hprose.Client\Debug\netcoreapp2.0

dotnet build .\proj\netcoreapp2.0\Hprose\Hprose-netcoreapp2.csproj
dotnet build .\proj\netcoreapp2.0\Hprose.Client\Hprose.Client-netcoreapp2.csproj