# AGENTS.md

This file provides guidance to Codex (Codex.ai/code) when working with code in this repository.

## Build & Test Commands

All commands run from `./framework/` (the solution directory):

```bash
# Restore dependencies
dotnet restore

# Build (Release)
dotnet build --configuration Release --no-restore

# Run all tests
dotnet test --no-restore --verbosity normal

# Pack NuGet packages
dotnet pack --no-restore --no-build --configuration Release --output nupkgs

# Full publish pipeline (PowerShell)
./framework/publish.ps1
```

To build/test a single project, pass the project path: `dotnet build framework/ZStack.Core/ZStack.Core.csproj`.

Templates have their own solution at `templates/ZStack.Framework.Templates.sln` and target older TFMs.

## Architecture

**Target framework**: `net10.0` (defined in `framework/Directory.Build.props`).

### Dependency graph

```
ZStack.Extensions (extension methods, zero deps)
  └── ZStack.Core (host builder, Serilog, YAML/INI config, DI attributes, RefAsync<T>)
        └── ZStack.AspNetCore (component system, AppStartup, Scalar, UnifyResult)
              ├── ZStack.AspNetCore.EventBus (EasyNetQ wrapper)
              ├── ZStack.AspNetCore.Hangfire (+ Redis/PostgreSql/MemoryStorage storage providers)
              ├── ZStack.AspNetCore.SqlSugar (ORM integration)
              ├── ZStack.AspNetCore.OpenTelemetry
              ├── ZStack.AspNetCore.QingTui
              └── ZStack.AspNetCore.Schedule.* (WIP — bin/obj only, no source yet)
```

### Component system (core architectural pattern)

Components are the primary extension mechanism. A component implements one or both of:

- **`IServiceComponent`** — `void Load(IServiceCollection services, ComponentContext ctx)` — registers services at startup.
- **`IApplicationComponent`** (marker interface) — gets invoked during `UseZStackInject()` to add middleware.

Components are auto-discovered from loaded assemblies. Control ordering and dependencies with attributes:

- **`[ComponentOrder(Order)]`** — lower order runs first.
- **`[DependsOn(typeof(OtherComponent), ...)]`** — ensures dependencies load before this component.
- **`[OptionsSection("SectionKey")]`** — binds a POCO options class to a `IConfiguration` section.

Each integration package (EventBus, Hangfire, etc.) exposes a component class that the host auto-discovers — no manual registration needed.

### Startup system

Classes inheriting `AppStartup` are also auto-discovered. They follow a convention-based pattern:
- Methods with signature `void ConfigureServices(IServiceCollection services)` are called during DI setup.
- Methods with signature `void Configure(IApplicationBuilder app, ...)` are called during middleware setup.
- Order controlled by `[AppStartup(Order)]`.

### DI auto-registration

Classes marked with `[Injection]` are auto-registered. The attribute controls:
- **`Pattern`**: `Self`, `FirstInterface`, `SelfWithFirstInterface`, `ImplementedInterfaces`, `All`
- **`Action`**: `Add` or `TryAdd`
- Lifecycle is determined by the interface the class implements: `ISingleton`, `IScoped`, or `ITransient`

### App static class

`ZStack.AspNetCore.App` is the global static accessor providing `ServiceProvider`, `Configuration`, `Logger`, `HostEnvironment`, `Assemblies`, `HttpContext`, `User`, and helper methods like `GetService<T>()`, `GetOptions<T>()`, `UseScope()`.

### Configuration loading

At startup, the framework scans the `Configuration/` directory (configurable via `ConfigurationDirectory` setting) for `*.json`, `*.ini`, `*.yml`, `*.yaml` files. Environment-specific files (e.g., `logger.Development.json`) are only loaded when the environment name matches. Also loads environment variables and command-line args.

JSON schemas for configuration are in `schemas/` (e.g., `eventbus.schema.json`, `hangfire.schema.json`, `sqlsugar.schema.json`).

### Entry points

**Console app:**
```csharp
var sp = AppHostBuilder.CreateHostBuilder(args).Build().Services;
```

**ASP.NET Core:**
```csharp
var builder = WebApplication.CreateBuilder(args).Inject();
// ... add services, controllers, etc.
app.UseZStackInject();
app.Run();
```

`Inject()` calls `InternalApp.ConfigureHostApplication`, sets up Serilog, discovers components, and runs DI auto-registration. `UseZStackInject()` invokes all `IApplicationComponent` implementations and `AppStartup` configure methods.

### AppStartup convention

```csharp
[AppStartup(Order = 1)]
public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services) { /* DI setup */ }
    public void Configure(IApplicationBuilder app, IHostEnvironment env) { /* middleware */ }
}
```

### Key utilities in ZStack.Core

- **`DI.AutoAddServices`** — the automatic DI scanning engine
- **`Scoped.Create/CreateAsync`** — create and manage DI scopes
- **`Reflection`** — assembly scanning utilities
- **`RefAsync<T>`** — boxed mutable reference type
- **`PerformanceTracker`** — performance measurement
- **`DateTimeConverter` / `TimeStampConverter`** — System.Text.Json converters
- **`TempFile`** — temporary file management
