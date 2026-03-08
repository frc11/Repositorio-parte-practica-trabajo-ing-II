# 1. Prueba de Unidad (Unit Testing)

## Concepto Teórico
La prueba de unidad centra el proceso de verificación en la unidad más pequeña del diseño de software, como un módulo o una función. Este enfoque de caja blanca se enfoca en la lógica de procesamiento interno y las estructuras de datos locales. Frecuentemente requiere el uso de programas "conductores" (drivers) para pasar datos de prueba y "resguardos" (stubs) para simular dependencias.

## Escenario Práctico en GarageWeb
En nuestro sistema de taller mecánico, tenemos una función en JavaScript que calcula el costo total de una orden de reparación. Esta función suma el costo de los repuestos, la mano de obra, y aplica un 21% de IVA. 

Para probarla de forma aislada, escribimos casos de prueba usando el framework **Jest**. Actuamos directamente sobre la interfaz de la función, asegurando que los caminos independientes y el manejo de errores (como introducir valores negativos) funcionen correctamente.

### Ejemplo de Implementación (JavaScript/Jest)

```javascript
// moduloFacturacion.js
export function calcularTotalReparacion(repuestos, manoDeObra) {
    if (repuestos < 0 || manoDeObra < 0) {
        throw new Error("Los valores no pueden ser negativos");
    }
    const subtotal = repuestos + manoDeObra;
    const iva = subtotal * 0.21;
    return subtotal + iva;
}

// moduloFacturacion.test.js (Nuestro Driver de prueba)
import { calcularTotalReparacion } from './moduloFacturacion';

describe('Pruebas Unitarias - calcularTotalReparacion', () => {
    test('Calcula el total correcto con IVA del 21%', () => {
        // 1000 + 500 = 1500. IVA (21%) = 315. Total = 1815
        expect(calcularTotalReparacion(1000, 500)).toBe(1815);
    });

    test('Lanza error si se ingresan valores negativos (Prueba de Límites)', () => {
        expect(() => calcularTotalReparacion(-100, 500)).toThrowError("Los valores no pueden ser negativos");
    });
});
