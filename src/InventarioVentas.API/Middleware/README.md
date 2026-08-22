# Middleware

Aqui se implementa comportamiento transversal que participa en el pipeline HTTP, por ejemplo el manejo uniforme de excepciones, logging tecnico o correlacion de solicitudes.

El middleware no reemplaza a los controllers ni a los services. Debe ocuparse del contexto HTTP y delegar la regla de negocio al componente que corresponda.

Al agregar un middleware, documenta su orden de registro en `Program.cs`, porque el orden puede cambiar el resultado de una solicitud.
