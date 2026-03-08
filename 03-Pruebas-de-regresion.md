# 3. Prueba de Regresión

## Concepto Teórico
La prueba de regresión es la ejecución repetida de pruebas previas. Su objetivo es asegurar que un cambio en el software (como agregar una nueva funcionalidad o corregir un bug) no propague efectos colaterales no deseados ni rompa funciones que antes operaban correctamente.

## Escenario Práctico
El mes pasado programamos y probamos la función `calcularTotalReparacion`. Hoy, el negocio nos pide agregar una nueva funcionalidad: **Descuentos para clientes frecuentes**. 

Modificamos el código de facturación para aceptar un tercer parámetro opcional (`descuento`). La prueba de regresión consiste en volver a ejecutar los tests unitarios y de integración originales mediante un pipeline de Integración Continua (CI/CD) para confirmar que la funcionalidad base (cálculo de IVA) sigue intacta.

### Ejemplo de Implementación (Modificación y Regresión)

```javascript
// moduloFacturacion.js modificado
export function calcularTotalReparacion(repuestos, manoDeObra, descuento = 0) {
    const subtotal = repuestos + manoDeObra;
    const subtotalConDescuento = subtotal - descuento;
    const iva = subtotalConDescuento * 0.21;
    return subtotalConDescuento + iva;
}

// Ejecución de Regresión (Jest en la terminal)
/*
> jest moduloFacturacion.test.js

PASS  ./moduloFacturacion.test.js
  Pruebas Unitarias - calcularTotalReparacion
    ✓ Calcula el total correcto con IVA del 21% (Test Antiguo - Regresión pasada)
    ✓ Lanza error si se ingresan valores negativos (Test Antiguo - Regresión pasada)
    ✓ Aplica el descuento correctamente antes del IVA (Test Nuevo)

Test Suites: 1 passed, 1 total
*/
