# Extensions

Esta carpeta contiene metodos de extension usados para registrar dependencias o agrupar configuraciones repetitivas de la aplicacion.

Es un lugar para composicion y configuracion, no para reglas de negocio. Un metodo de extension puede registrar un service, un validator, el `DbContext` o middleware, pero no debe ejecutar operaciones de negocio durante el arranque.

Cuando agregues un registro, mantenlo cerca del modulo o infraestructura que configura y actualiza `Program.cs` de forma explicita.
