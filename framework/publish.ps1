Write-Host "🌐 还原NuGet包..."
dotnet restore

Write-Host "🛠️ 编译..."
dotnet build --configuration Release --no-restore

Write-Host "🧪 测试..."
dotnet test --no-restore --verbosity normal

Write-Host "🗂️ 打包..."
dotnet pack --no-restore --no-build --configuration Release --output nupkgs

Write-Host "🚀 发布到NuGet..."
cd nupkgs
dotnet nuget push *.nupkg --source "https://api.nuget.org/v3/index.json" --api-key $env:NUGET_API_KEY --skip-duplicate

Write-Host "✅ 发布完成！"
