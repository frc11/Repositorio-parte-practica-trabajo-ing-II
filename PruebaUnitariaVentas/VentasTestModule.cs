using System;
using System.Collections.Generic;
using System.Linq;

namespace Polleria.Tests
{
    // Archivo de pruebas aislado para probar los calculos de Ventas
    // Contiene unicamente lo fundamental para que calcular el costo total y el precio total funcione.

    public class Ventas
    {
        public long IdVenta { get; set; }
        public decimal Redondeo { get; set; }
        public List<DetallesVentas> DetallesVenta { get; set; } = new List<DetallesVentas>();
        public List<VentasPromociones> VentaPromociones { get; set; } = new List<VentasPromociones>();

        public void AgregarDetalle(DetallesVentas detalle)
        {
            DetallesVenta.Add(detalle);
        }

        public void AgregarPromocion(VentasPromociones promociones)
        {
            VentaPromociones.Add(promociones);
        }

        public decimal CalcularCostoTotal()
        {
            return DetallesVenta.Sum(detalle => detalle.CalcularCosto()) + VentaPromociones.Sum(promocion => promocion.CalcularCosto());
        }

        public decimal CalcularPrecioTotal()
        {
            return DetallesVenta.Sum(detalle => detalle.CalcularPrecio()) + VentaPromociones.Sum(promocion => promocion.CalcularPrecio()) + Redondeo;
        }
    }

    public class DetallesVentas
    {
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal PrecioUnitario { get; set; }

        public DetallesVentas(int idProducto, decimal cantidad, decimal costoUnitario, decimal precioUnitario)
        {
            IdProducto = idProducto;
            Cantidad = cantidad;
            CostoUnitario = costoUnitario;
            PrecioUnitario = precioUnitario;
        }

        public decimal CalcularCosto()
        {
            return CostoUnitario * Cantidad;
        }

        public decimal CalcularPrecio()
        {
            return PrecioUnitario * Cantidad;
        }
    }

    public class VentasPromociones
    {
        public int IdPromocion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoPromo { get; set; }
        public decimal PrecioPromo { get; set; }

        public VentasPromociones(int idPromocion, decimal cantidad, decimal costoPromo, decimal precioPromo)
        {
            IdPromocion = idPromocion;
            Cantidad = cantidad;
            CostoPromo = costoPromo;
            PrecioPromo = precioPromo;
        }

        public decimal CalcularCosto()
        {
            return CostoPromo * Cantidad;
        }

        public decimal CalcularPrecio()
        {
            return PrecioPromo * Cantidad;
        }
    }

    // Ejemplo de uso para probar el modulo:
    public class VentasTestsRunner
    {
        public static void RunTests()
        {
            // Setup
            var venta = new Ventas
            {
                IdVenta = 1,
                Redondeo = -0.50m
            };

            venta.AgregarDetalle(new DetallesVentas(idProducto: 1, cantidad: 2m, costoUnitario: 100m, precioUnitario: 150m));
            venta.AgregarDetalle(new DetallesVentas(idProducto: 2, cantidad: 1m, costoUnitario: 50m, precioUnitario: 70m));
            
            venta.AgregarPromocion(new VentasPromociones(idPromocion: 1, cantidad: 1m, costoPromo: 200m, precioPromo: 250m));

            // Calculos
            // Costo esperado: (2 * 100) + (1 * 50) + (1 * 200) = 200 + 50 + 200 = 450
            decimal costoTotal = venta.CalcularCostoTotal();
            
            // Precio esperado: (2 * 150) + (1 * 70) + (1 * 250) + Redondeo(-0.50) = 300 + 70 + 250 - 0.50 = 619.50
            decimal precioTotal = venta.CalcularPrecioTotal();

            Console.WriteLine($"Costo Total Calculado: {costoTotal} | Esperado: 450");
            Console.WriteLine($"Precio Total Calculado: {precioTotal} | Esperado: 619.50");
        }
    }
}
