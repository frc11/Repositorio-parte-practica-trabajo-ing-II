# 2. Prueba de Integración

## Concepto Teórico
Una vez que los módulos funcionan individualmente, la prueba de integración busca construir la arquitectura del software de forma incremental, descubriendo errores en las interfaces y en la interacción entre componentes. Se evita el enfoque "Big Bang" a favor de integraciones ascendentes o descendentes.

## Escenario Práctico
Aplicaremos un **enfoque incremental ascendente**. Comenzamos probando el módulo de más bajo nivel: nuestra función que ejecuta consultas `SQL` para guardar un nuevo cliente en la base de datos. Luego, la integramos "hacia arriba" con nuestra ruta de API de Next.js (el controlador).

El objetivo es verificar que los datos JSON que envía la API se transformen correctamente en una consulta SQL sin pérdida de datos en la interfaz entre ambos módulos.

### Ejemplo de Implementación (Next.js API + SQL)

```javascript
// db/clientes.js (Módulo de nivel bajo)
import { sql } from '@vercel/postgres';

export async function insertarClienteBD(nombre, email) {
    return await sql`INSERT INTO clientes (nombre, email) VALUES (${nombre}, ${email}) RETURNING *`;
}

// api/clientes/route.js (Módulo de nivel alto integrado)
import { insertarClienteBD } from '../../db/clientes';

export async function POST(request) {
    const data = await request.json();
    const result = await insertarClienteBD(data.nombre, data.email);
    return Response.json({ success: true, cliente: result.rows[0] });
}

// integracion.test.js
test('Integración API-BaseDeDatos: Guarda un cliente correctamente', async () => {
    // Simulamos un request a la API
    const mockRequest = { json: async () => ({ nombre: "Valentino Olmedo", email: "valenolme@gmail.com" }) };
    
    // Ejecutamos la ruta integrada
    const response = await POST(mockRequest);
    const body = await response.json();

    // Verificamos que la interfaz entre la API y SQL funcionó
    expect(body.success).toBe(true);
    expect(body.cliente.nombre).toBe("Juan Perez");
});
