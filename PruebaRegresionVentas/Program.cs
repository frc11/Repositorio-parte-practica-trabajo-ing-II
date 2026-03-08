using Microsoft.EntityFrameworkCore;

using entornoPolleria;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

// Agregar todos los repositorios para Dependency Injection
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("VentasTestDB"));

// Inyectar los Repositorios de Ventas y todas sus dependencias relacionadas para las Vistas
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IProductosRepository, ProductosRepository>();
builder.Services.AddScoped<IPromocionesRepository, PromocionesRepository>();
builder.Services.AddScoped<IMetodosPagoRepository, MetodosPagoRepository>();

var app = builder.Build();

// Llenar Base de Datos con Datos de Prueba (Seed Data)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Crear métodos de pago base requeridos por el form
    if (!context.MetodosPago.Any())
    {
        context.MetodosPago.AddRange(
            new MetodosPago { IdMetodo = 1, Metodo = "Efectivo" },
            new MetodosPago { IdMetodo = 2, Metodo = "Tarjeta Débito" },
            new MetodosPago { IdMetodo = 3, Metodo = "MercadoPago" }
        );
        context.SaveChanges();
    }

    // Crear Productos Base
    if (!context.Productos.Any())
    {
        context.Productos.AddRange(
            new Productos { IdProducto = 1, Producto = "Pollo Entero", Precio = 5000, Costo = 3000, Stock = 100, Activo = true },
            new Productos { IdProducto = 2, Producto = "Papas Fritas", Precio = 2000, Costo = 800, Stock = 50, Activo = true }
        );
        context.SaveChanges();
    }

    // Crear Promociones Base
    if (!context.Promociones.Any())
    {
        var promo = new Promociones { IdPromocion = 1, Promocion = "Combo Pollo con Papas", Precio = 6500, Inicio = DateOnly.FromDateTime(DateTime.Now), Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(30)) };
        context.Promociones.Add(promo);
        context.SaveChanges();
        
        context.DetallesPromociones.AddRange(
            new DetallesPromociones { IdPromocion = 1, IdProducto = 1, Cantidad = 1 },
            new DetallesPromociones { IdPromocion = 1, IdProducto = 2, Cantidad = 1 }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

// Que por defecto inicie en Ventas/Index para que el test arranque directo ahí
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ventas}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
