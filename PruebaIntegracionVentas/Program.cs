using System;
using Polleria.Tests.Integracion;

namespace PruebaIntegracionVentas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine(" INICIANDO PRUEBA DE INTEGRACION: VENTAS CONTROLLER");
            Console.WriteLine("==================================================\n");

            // 1. Instanciamos el controlador simulado
            var controller = new VentasControllerMock();

            // 2. Preparamos una entidad Venta compleja
            var nuevaVenta = new Ventas { Redondeo = -0.50m };
            nuevaVenta.AgregarDetalle(new DetallesVentas(idProducto: 1, cantidad: 2m, costoUnitario: 100m, precioUnitario: 150m));
            nuevaVenta.AgregarPromocion(new VentasPromociones(idPromocion: 1, cantidad: 1m, costoPromo: 200m, precioPromo: 250m));

            // 3. PRUEBA DE INTEGRACIÓN: ALTA DE VENTA
            Console.WriteLine(">> TRATANDO DE DAR DE ALTA UNA VENTA...");
            string resultadoAlta = controller.Alta(nuevaVenta);
            Console.WriteLine($"RESULTADO DE LA ACCIÓN: {resultadoAlta}\n");

            // 4. PRUEBA DE INTEGRACIÓN: LISTADO DE VENTAS
            Console.WriteLine(">> TRAYENDO TODAS LAS VENTAS GUARDADAS (INDEX)...");
            var ventasGuardadas = controller.ObtenerTodas();
            Console.WriteLine($"CANTIDAD EN BASE DE DATOS MOCK: {ventasGuardadas.Count}");
            foreach(var v in ventasGuardadas)
            {
                Console.WriteLine($"- Venta ID: {v.IdVenta} | Precio Total Calculado por Modelos: ${v.CalcularPrecioTotal()}");
            }
            Console.WriteLine();

            // 5. PRUEBA DE INTEGRACIÓN: ELIMINAR VENTA
            Console.WriteLine(">> TRATANDO DE ELIMINAR LA VENTA CON ID 1...");
            string resultadoEliminar = controller.EliminarConfirmado(1);
            Console.WriteLine($"RESULTADO DE LA ACCIÓN: {resultadoEliminar}\n");

            // Verificamos que se borro
            Console.WriteLine(">> REVISANDO BASE DE DATOS TRAS ELIMINAR...");
            Console.WriteLine($"CANTIDAD RESTANTE: {controller.ObtenerTodas().Count}\n");

            Console.WriteLine("==================================================");
            Console.WriteLine(" PRUEBA DE INTEGRACIÓN FINALIZADA");
            Console.WriteLine("==================================================");
        }
    }
}
