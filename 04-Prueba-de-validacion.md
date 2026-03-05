# 4. Prueba de Validación

## Concepto Teórico
Una vez que el software está integrado, la prueba de validación consiste en pruebas de caja negra a gran escala para demostrar la conformidad del sistema con los requisitos del cliente. Esto responde a la pregunta: *¿Estamos construyendo el producto correcto?*. Incluye las pruebas Alfa (en el entorno del desarrollador) y Beta (en el entorno del cliente final).

## Escenario Práctico en GarageWeb
El sistema GarageWeb está desplegado en un servidor de pruebas (Staging). El cliente (el dueño del taller mecánico) realiza una **Prueba Alfa** controlada. 

Para automatizar esta prueba de validación a nivel interfaz gráfica de usuario (GUI), utilizamos una herramienta de pruebas E2E (End-to-End) como **Cypress** o **Playwright**. El script simula a un usuario humano navegando por la web, llenando el formulario de nuevo servicio y verificando que el mensaje de éxito aparezca en pantalla.

### Ejemplo de Implementación (Cypress E2E)

```javascript
// validacion_reserva.spec.js
describe('Prueba de Validación: Flujo de Nuevo Servicio', () => {
    it('Permite al administrador cargar una reparación y muestra el mensaje de éxito', () => {
        // 1. Visitar la página web principal
        cy.visit('[https://staging.ejemplo.com/admin/nuevo-servicio](https://staging.ejemplo.com/admin/nuevo-servicio)');

        // 2. Interactuar con la interfaz gráfica (Caja Negra pura)
        cy.get('input[name="patente"]').type('AB123CD');
        cy.get('input[name="falla_cliente"]').type('Frenos gastados');
        cy.get('input[name="costo_estimado"]').type('50000');
        
        // 3. Simular el click en el botón de guardar
        cy.get('button[type="submit"]').click();

        // 4. Validar el requisito de negocio: Redirección y mensaje de confirmación
        cy.url().should('include', '/admin/servicios-activos');
        cy.contains('El servicio fue registrado exitosamente.').should('be.visible');
    });
});
