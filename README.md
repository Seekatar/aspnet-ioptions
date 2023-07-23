# IOptions Test Program

This was started with `dotnet new webapi`

This tests the three flavors of [IOptions](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options), and [Validation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0#options-validation), and injection to services early on.

## Notes

`Startup.ConfigureService` creates the options and a couple services that need them as parameters for their constructors.

`Startup.Configure` takes all the options and validates them early since validation is done when `Value` is referenced. If only you only inject into controllers, you won't know until the controller is called, which could be much later.

`TestService` takes option classes in its constructor and is new'd up in `ConfigureServices`

`TestServiceInject` takes IOptions in its constructor and it constructed when injected.

The controller takes everything in its constructor.

Note that you can change `appsetting.json` and valued in `MonitorOptions` and `SnapshotOptions` will be reflected.

`IOptions` and `IOptionsMonitor` are singletons.
