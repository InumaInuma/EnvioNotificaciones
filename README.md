# EnvioNotificaciones
Notificador de Citas por WhatsApp (Worker Service)
Este proyecto es un Worker Service robusto y eficiente, diseñado para automatizar el envío de notificaciones y recordatorios de citas vía WhatsApp. Implementado con .NET 8, sigue rigurosamente los principios de la Arquitectura Limpia (Clean Architecture) y utiliza la Inyección de Dependencias para una estructura modular, mantenible y escalable.

Características Principales
Worker Service de .NET 8: Una aplicación de fondo diseñada para ejecutar tareas programadas de forma confiable, ideal para procesos continuos como el envío de notificaciones.

Arquitectura Limpia (Clean Architecture): El proyecto está estructurado en capas bien definidas (Dominio, Aplicación, Infraestructura, Contratos Compartidos), asegurando una clara separación de responsabilidades y facilitando la mantenibilidad, la testabilidad y la evolución del software.

Inyección de Dependencias (DI): Utiliza un contenedor de DI para gestionar las dependencias entre componentes, promoviendo el acoplamiento débil y la flexibilidad.

Gestión de Citas: Se encarga de consultar, procesar y actualizar el estado de las citas en una base de datos.

Base de Datos SQL Server con Procedimientos Almacenados: La interacción con la base de datos se realiza a través de procedimientos almacenados optimizados, lo que garantiza eficiencia en las operaciones de consulta y actualización de datos de citas.

Consumo de la API de WhatsApp Business de Meta: Integra y utiliza la API oficial de WhatsApp Business de Meta para enviar mensajes programados a los usuarios, proporcionando una comunicación fiable y directa.

Componentes Clave
EnvioNotificacionesDomian: Define el núcleo del negocio: entidades, interfaces de repositorio y reglas de dominio.

EnvioNotificacionesApplication: Contiene la lógica de la aplicación y orquesta las operaciones del dominio a través de servicios de aplicación.

EnvioNotificacionesInfrastructure: Implementa los detalles técnicos: acceso a datos (Entity Framework Core con SQL Server), servicios externos (integración con la API de WhatsApp de Meta) e implementaciones de los repositorios.

WhatsAppNotifierWorkerService: El proyecto ejecutable del Worker Service, que aloja la lógica de fondo y orquesta la ejecución programada de las tareas de notificación.

EnvioNotificacionesUIWinForms: Una aplicación de escritorio WinForms opcional que proporciona una interfaz de usuario sencilla para controlar (iniciar/detener) el Worker Service en la bandeja del sistema.

Despliegue
Este Worker Service está diseñado para ser desplegado como un Servicio de Windows, lo que asegura su ejecución persistente en segundo plano y su fácil gestión a través del Administrador de Servicios de Windows.
