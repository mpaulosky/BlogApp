using FluentAssertions;

using Microsoft.Extensions.Logging;

using static BlogApp.Domain.Constants.ServiceNames;

namespace BlogApp.AppHost.Tests;

public class IntegrationTests
{

	private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);


	[Fact]
	public async Task GetWebResourceRootReturnsOkStatusCode()
	{

		// Arrange
		var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.BlogApp_AppHost>();

		appHost.Services.AddLogging(logging =>
		{
			logging.SetMinimumLevel(LogLevel.Debug);

			// Override the logging filters from the app's configuration
			logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
			logging.AddFilter("Aspire.", LogLevel.Debug);

			// To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
		});

		appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
		{
			clientBuilder.AddStandardResilienceHandler();
		});

		await using var app = await appHost.BuildAsync().WaitAsync(_defaultTimeout);

		await app.StartAsync().WaitAsync(_defaultTimeout);

		// Act
		var httpClient = app.CreateHttpClient(WebApp);

		await app.ResourceNotifications.WaitForResourceHealthyAsync(WebApp).WaitAsync(_defaultTimeout);

		var response = await httpClient.GetAsync("/");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

	}

}
