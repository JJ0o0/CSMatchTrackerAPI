using CSMatchTracker.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.NumberHandling =
                    JsonNumberHandling.Strict;

                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

        builder.Services.AddOpenApi(options => {
            options.AddOperationTransformer((operation, context, cancellationToken) => {
                foreach (var parameter in operation.Parameters ?? []) {
                    if (parameter.In == ParameterLocation.Path &&
                        parameter.Name == "id" &&
                        parameter.Schema is OpenApiSchema schema) {

                        schema.Type = JsonSchemaType.Integer;
                        schema.Format = "int64";
                        schema.Pattern = null;
                    }
                }

                return Task.CompletedTask;
            });

            options.AddDocumentTransformer((document, context, cancellationToken) => {
                document.Components ??= new OpenApiComponents();

                document.Components.SecuritySchemes ??=
                    new Dictionary<string, IOpenApiSecurityScheme>();

                document.Components.SecuritySchemes["Bearer"] =
                    new OpenApiSecurityScheme {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Name = "Authorization"
                    };

                foreach (var path in document.Paths.Values) {
                    foreach (var operation in path.Operations.Values) {
                        if (operation.Security is not null) {
                            operation.Security.Add(
                                new OpenApiSecurityRequirement {
                                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                                }
                            );
                        }
                    }
                }

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, cancellationToken) => {
                if (context.Description.ActionDescriptor.EndpointMetadata
                    .OfType<AuthorizeAttribute>()
                    .Any()) {

                    operation.Security ??= [];

                    operation.Security.Add(
                        new OpenApiSecurityRequirement {
                            [new OpenApiSecuritySchemeReference(
                                "Bearer",
                                context.Document
                            )] = []
                        }
                    );
                }

                return Task.CompletedTask;
            });
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.Authority = "https://rqsufihdnqwsltrrzhti.supabase.co/auth/v1";
                options.Audience = "authenticated";
            });

        builder.Services.AddAuthorization(options => {
            options.AddPolicy("Admin", policy =>
                policy.RequireClaim("user_role", "admin"));
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint(
                    "/openapi/v1.json",
                    "CSMatchTracker API v1"
                );
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}