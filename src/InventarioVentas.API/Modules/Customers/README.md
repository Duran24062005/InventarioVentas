# Modulo Clientes

Este modulo registra y consulta los clientes que pueden realizar ventas. Es propietario de los datos y reglas de identificacion del cliente.

## Que debe ir aqui

- Endpoints de clientes.
- DTOs de clientes.
- Entidad `Cliente`.
- Contratos y services de clientes.
- Validadores de las solicitudes de clientes.

El documento funcional exige nombre obligatorio, documento unico y email valido. Ventas puede verificar que un cliente exista, pero no debe duplicar la logica de registro o actualizacion de clientes.

La estructura esta creada y sirve como guia para la implementacion posterior.
