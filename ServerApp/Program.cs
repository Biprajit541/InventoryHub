using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    const string cacheKey = "products";

    if (!cache.TryGetValue(cacheKey, out object? products))
    {
        products = new[]
        {
            new
            {
                Id = 1,
                Name = "Laptop",
                Price = 1200.50,
                Stock = 25,
                Category = new
                {
                    Id = 101,
                    Name = "Electronics"
                }
            },
            new
            {
                Id = 2,
                Name = "Headphones",
                Price = 50.00,
                Stock = 100,
                Category = new
                {
                    Id = 102,
                    Name = "Accessories"
                }
            }
        };

        cache.Set(cacheKey, products, TimeSpan.FromMinutes(5));
    }

    return Results.Json(products);
});

app.Run();