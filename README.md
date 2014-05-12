# WebApi-AuthenticationFilter


The missing **AuthenticationFilterAttribute** from ASP.NET Web API 2.

## Motivation

ASP.NET Web API 2 has introduced a new filter interface, [IAuthenticationFilter](http://msdn.microsoft.com/en-us/library/system.web.http.filters.iauthenticationfilter.aspx), which can be used to provide custom authentication to APIs powered by the framework. If you used the `IAuthorizationFilter` interface in the past to implement both authentication and authorization, now you can split those two concerns into separate classes.

But contrary to what you may expect, there's no AuthenticationFilterAttribute class to extend, as you would do for all other filters. [According to Web API devs](http://stackoverflow.com/questions/23554515/why-isnt-there-an-authenticationfilterattribute-class-in-asp-net-web-api-2), that's by design since they don't expect the average user to use an authentication filter.

So that's what this simple project is all about: to fill that gap and provide an **AuthenticationFilterAttribute** class that you can use the same way you use all other Web API filters.

## Installation

To install WebApi.AuthenticationFilter, run the following command in the Package Manager Console inside Visual Studio:

    Install-Package WebApi.AuthenticationFilter

The package currently depends on [Microsoft ASP.NET Web API 2.1 Core](https://www.nuget.org/packages/Microsoft.AspNet.WebApi.Core/) (≥ 5.0)

## Basic usage

You can use the `AuthenticationFilterAttribute` class either synchronously or asynchronously:

### Synchronously

```csharp
using WebApi.AuthenticationFilter;

public class AuthenticationFilter : AuthenticationFilterAttribute
{
	public override void OnAuthentication(HttpAuthenticationContext context)
	{
        if (!Authenticate(context))
        {
            context.ErrorResult = new StatusCodeResult(HttpStatusCode.Unauthorized, context.Request);
        }
	}

    private bool Authenticate(HttpAuthenticationContext context)
    {
        // Authenticates the request 
    }
}
```

### Asynchronously

```csharp
using WebApi.AuthenticationFilter;

public class AuthenticationFilter : AuthenticationFilterAttribute
{
	public async override Task OnAuthenticationAsync(HttpAuthenticationContext context, 
        CancellationToken cancellationToken)
	{            
        if (!await Authenticate(context))
        {
            context.ErrorResult = new StatusCodeResult(HttpStatusCode.Unauthorized, context.Request);
        }            
	}

    private Task<bool> Authenticate(HttpAuthenticationContext context)
    {
        // Authenticates the request 
    }
}
```

Now all you have to do is register your filter, typically in the `WebApiConfig.cs` file:

```csharp
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {     
        config.Filters.Add(new AuthenticationFilter());
    }
}
```