$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$templatesRoot = $PSScriptRoot
$webApiTemplate = Join-Path $templatesRoot "ZStackWebApi\src"
$consoleTemplate = Join-Path $templatesRoot "ConsoleApp\src"
$workRoot = Join-Path ([IO.Path]::GetTempPath()) ("zstack-template-validation-" + [Guid]::NewGuid().ToString("N"))
$installedWebApi = $false
$installedConsole = $false

function Invoke-Dotnet {
    param(
        [Parameter(Mandatory)] [string[]] $Arguments
    )

    & dotnet @Arguments | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') 失败，退出码：$LASTEXITCODE"
    }
}

function Assert-FileExists {
    param([Parameter(Mandatory)] [string] $Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "缺少预期文件：$Path"
    }
}

function Assert-FileMissing {
    param([Parameter(Mandatory)] [string] $Path)

    if (Test-Path -LiteralPath $Path) {
        throw "不应生成文件：$Path"
    }
}

function Assert-TextContains {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Text
    )

    if (-not (Get-Content -Raw -LiteralPath $Path).Contains($Text)) {
        throw "文件 $Path 不包含预期文本：$Text"
    }
}

function Assert-TextMissing {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Text
    )

    if ((Get-Content -Raw -LiteralPath $Path).Contains($Text)) {
        throw "文件 $Path 包含不应出现的文本：$Text"
    }
}

function New-WebApiSample {
    param(
        [Parameter(Mandatory)] [string] $Name,
        [string[]] $TemplateArguments = @()
    )

    $output = Join-Path $workRoot $Name
    Invoke-Dotnet (@("new", "zstack-webapi", "--name", $Name, "--output", $output) + $TemplateArguments)
    return $output
}

try {
    New-Item -ItemType Directory -Path $workRoot | Out-Null

    Invoke-Dotnet @("new", "install", $consoleTemplate, "--force")
    $installedConsole = $true
    Invoke-Dotnet @("new", "install", $webApiTemplate, "--force")
    $installedWebApi = $true

    $consoleOutput = Join-Path $workRoot "ConsoleSample"
    Invoke-Dotnet @("new", "zstack-console", "--name", "ConsoleSample", "--output", $consoleOutput)
    $consoleProject = Join-Path $consoleOutput "ConsoleSample.csproj"
    Assert-FileExists $consoleProject
    Assert-TextContains $consoleProject "TargetFramework>net10.0"
    Assert-TextContains $consoleProject "ZStack.Core"
    Invoke-Dotnet @("restore", $consoleProject)
    Invoke-Dotnet @("build", $consoleProject, "--configuration", "Release", "--no-restore")

    $minimal = New-WebApiSample "MinimalApi" @()
    $minimalProject = Join-Path $minimal "MinimalApi.csproj"
    Assert-FileExists $minimalProject
    Assert-FileExists (Join-Path $minimal "Controllers\HealthController.cs")
    Assert-FileMissing (Join-Path $minimal "Configuration\cache.json")
    Assert-FileMissing (Join-Path $minimal "Examples\CacheExampleController.cs")
    Assert-TextContains $minimalProject "ZStack.AspNetCore"
    Assert-TextMissing $minimalProject "ZStack.AspNetCore.EventBus"
    Assert-TextMissing $minimalProject "ZStack.AspNetCore.Hangfire"
    Assert-TextMissing $minimalProject "Furion"

    $all = New-WebApiSample "AllComponentsApi" @(
        "--components", "cache",
        "--components", "eventbus",
        "--components", "hangfire",
        "--components", "sqlsugar",
        "--components", "opentelemetry",
        "--components", "qingtui",
        "--hangfireStorage", "memory"
    )
    $allProject = Join-Path $all "AllComponentsApi.csproj"
    foreach ($file in @(
        "Configuration\cache.json",
        "Configuration\eventbus.json",
        "Configuration\hangfire.json",
        "Configuration\sqlsugar.json",
        "Configuration\opentelemetry.json",
        "Configuration\qingtui.json",
        "Examples\CacheExampleController.cs",
        "Examples\EventBusExampleController.cs",
        "Examples\HangfireExampleJob.cs",
        "Examples\SqlSugarExampleController.cs",
        "Examples\OpenTelemetryExampleController.cs",
        "Examples\QingTuiExampleController.cs"
    )) {
        Assert-FileExists (Join-Path $all $file)
    }
    foreach ($package in @(
        "ZStack.AspNetCore.EventBus",
        "ZStack.AspNetCore.Hangfire",
        "ZStack.AspNetCore.Hangfire.MemoryStorage",
        "ZStack.AspNetCore.SqlSugar",
        "ZStack.AspNetCore.OpenTelemetry",
        "ZStack.AspNetCore.QingTui"
    )) {
        Assert-TextContains $allProject $package
    }
    Assert-TextMissing $allProject "ZStack.AspNetCore.Hangfire.Redis"

    $redis = New-WebApiSample "HangfireRedisApi" @(
        "--components", "hangfire",
        "--hangfireStorage", "redis"
    )
    $redisProject = Join-Path $redis "HangfireRedisApi.csproj"
    Assert-TextContains $redisProject "ZStack.AspNetCore.Hangfire.Redis"
    Assert-TextMissing $redisProject "ZStack.AspNetCore.Hangfire.MemoryStorage"

    Invoke-Dotnet @("restore", $minimalProject)
    Invoke-Dotnet @("build", $minimalProject, "--configuration", "Release", "--no-restore")
    Invoke-Dotnet @("restore", $allProject)
    Invoke-Dotnet @("build", $allProject, "--configuration", "Release", "--no-restore")

    Write-Host "模板验证通过。"
}
finally {
    if ($installedWebApi) {
        & dotnet new uninstall $webApiTemplate *> $null
    }
    if ($installedConsole) {
        & dotnet new uninstall $consoleTemplate *> $null
    }
    if (Test-Path -LiteralPath $workRoot) {
        Remove-Item -LiteralPath $workRoot -Recurse -Force
    }
}
