# Responses

Contiene modelos para respuestas HTTP compartidas cuando la API necesita un formato consistente para datos, errores o metadatos.

Los modelos de esta carpeta son contratos comunes y deben mantenerse pequenos. No deben contener entidades de Entity Framework ni reglas para calcular precios, stock, ventas u otra funcionalidad de negocio.

Si un endpoint necesita una respuesta exclusiva de su modulo, define el DTO en `Modules/<Modulo>/DTOs`.
