using System;
using System.Collections.Generic;
using System.Linq;

namespace Polleria.Tests.Integracion
{
    // Archivo de pruebas de integración para aislar VentasController y Ventas.cs
    // Reemplaza MVC (IActionResult, HTTP Requests) por métodos directos.

    public class VentasControllerMock
    {
        private List<Ventas> _mockDatabaseVentas = new List<Ventas>();

        // Simulación de Alta (POST)
        public string Alta(Ventas nuevaVenta)
        {
            try
            {
                // Simulamos guardar en base de datos
                nuevaVenta.IdVenta = _mockDatabaseVentas.Count + 1;
                _mockDatabaseVentas.Add(nuevaVenta);
                
                return $"SUCCESS: Venta {nuevaVenta.IdVenta} registrada con éxito con un total de ${nuevaVenta.CalcularPrecioTotal()}";
            }
            catch (Exception e)
            {
                return $"ERROR: Ocurrió un error inesperado al registrar la venta. Excepcion: {e.Message}";
            }
        }

        // Simulación de Eliminar (POST)
        public string EliminarConfirmado(long idVenta)
        {
            try
            {
                var venta = _mockDatabaseVentas.FirstOrDefault(v => v.IdVenta == idVenta);
                if (venta != null)
                {
                    _mockDatabaseVentas.Remove(venta);
                    return $"SUCCESS: Venta {idVenta} eliminada con éxito.";
                }
                return $"ERROR: La venta {idVenta} no fue encontrada.";
            }
            catch (Exception e)
            {
                return $"ERROR: Ocurrió un error al intentar eliminar la venta. Excepcion: {e.Message}";
            }
        }

        // Simulación de Listado (GET Index)
        public List<Ventas> ObtenerTodas()
        {
            return _mockDatabaseVentas;
        }
    }

    // -------------------------------------------------------------
    // MODELOS REUTILIZADOS (IDÉNTICOS AL TEST AISLADO ANTERIOR)
    // -------------------------------------------------------------
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
}
