// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ResourceBuilderExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : BlogApp
// Project Name :  BlogApp.AppHost
// =======================================================

namespace BlogApp.AppHost;

internal static class ResourceBuilderExtensions
{

	internal static IResourceBuilder<T> WithSwaggerUi<T>(this IResourceBuilder<T> builder)
			where T : IResourceWithEndpoints
	{

		return builder.WithOpenApiDocs("swagger-ui-docs", "Swagger Api Documentation", "swagger");

	}

	internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
			where T : IResourceWithEndpoints
	{

		return builder.WithOpenApiDocs("scalar-docs", "Scalar Api Documentation", "scalar/v1");

	}

	private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,
			string name,
			string displayName,
			string openApiPath)
			where T : IResourceWithEndpoints
	{

		return builder.WithCommand(
				name,
				displayName,
				executeCommand: async _ =>
				{
					try
					{

						// Base URL for the Swagger UI
						var endpoint = builder.GetEndpoint("https");

						var url = $"{endpoint.Url}/{openApiPath}";

						Process.Start(new ProcessStartInfo(url)
						{
								UseShellExecute = true,
								Verb = "open"
						});

						return new ExecuteCommandResult { Success = true };

					}
					catch (Exception e)
					{

						Console.WriteLine(e);

						return new ExecuteCommandResult { Success = false, ErrorMessage = e.Message };

					}

				},
				updateState: context =>
						context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
								? ResourceCommandState.Enabled
								: ResourceCommandState.Disabled,
				iconName: "Document",
				iconVariant: IconVariant.Filled);

	}

}
