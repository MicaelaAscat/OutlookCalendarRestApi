# OutlookCalendarRestApi
Api REST que permite listar, crear, editar y eliminar eventos de un calendario de Outlook.

## Demo
https://www.loom.com/share/f8f221f20b8640a3a086b33a0b3f49b0

## Url OutlookCalendarRestApi Swagger
https://codacalendarapi.azurewebsites.net/swagger/index.html

## Github Repo
https://github.com/MicaelaAscat/OutlookCalendarRestApi.git

## Autenticación 
La autenticación se realizó utilizando Microsoft Identity. Siguiendo el protocolo Oauth2, para poder ejecutar un endpoint se necesita un acces token. La forma más simple de probarlo es ingresar a swagger, presionar el botón "Authorize", seleccionar el scope requerido y luego loguearse con la cuenta de Microsoft cuyo calendario se quiera obtener. 
Una vez autenticado, retorna a swagger con un popup que indica que esta logueado, cerralo y ya se puede acceder a la API.

## Conexión con Microsoft Graph 
La conexion con Microsoft Graph se realizó utilizando el flujo OBO (On-Behalf-Of). El request a la API OutlookCalendarRestApi llega con un token otorgado para que el usuario para acceder. Este token es utilizado para obtener un nuevo access token con los permisos necesarios para acceder a Microsoft Graph. 
Más información sobro el flujo OBO en: https://docs.microsoft.com/es-es/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow

#### Cacheo de tokens
Se configuró el cacheo de los tokens para el acceso Microsoft Graph en una base de datos Sql Server. 
La bd es coda_calendar_api y la tabla TokenCache.

## Endpoints
### GET /Authorization
Como este ejercicio se trataba de realizar una API, el flujo de delegación de permisos no se puede realizar completo y se necesita que el usuario haga algunas tareas manuales. Para eso se generó este endpoint, el cual retorna una url para que el usuario pueda otorgar los permisos que requiere la api para acceder a su calendario de Outlook. La url retornada se debe copiar y pegar en el browser. Se observará una página de autorización de permisos requeridos para manipular el calendario. Luego de aceptar, redireccionará a una url que no existe (redirect url), lo cual no es un inconveniente ya que estamos usando los endpoints de la API directamente.
Este flujo es mucho más claro cuando se tiene un sitio web que hace de intermediario entre la API y el Identity Provider.
Ya con los permisos otorgados, se pueden probar el resto de endpoints.

### GET /Events
Retorna los eventos del calendario del usuario logueado, ordenados por fecha en forma descendente. 
(Nota: se retornan los primeros 50, no se realizó paginado por cuestion de tiempos).

### POST /Events
Crea un nuevo evento en el calendario.

### PATCH /Events
Dado el id de un evento existente, actualiza sus propiedades.

### DELETE /Events
Dado el id de un evento existente, se elimina del calendario.

## Componentes del proyecto
Los siguientes archivos contienen código relacionado con la conexión a Microsoft Graph.

#### appsettings.json
Contiene los valores utilizados para autenticación y autorización.

#### Startup.cs 
Configura la aplicación y los servicios que utiliza, incluida la autenticación.

### Controladores

#### AuthorizationController.cs 
Para poder solicitar los permisos para acceder al calendario.

####  CalendarController.cs 
Para controlar las solicitudes al calendario.

### Modelo

#### EventDto
Dto que representa los conceptos básicos de un evento de calendario. Se intentó abstraer el concepto del dominio de la implementación de Outlook.

## Servicios
#### ICalendarService
Interfaz que representa la comunicación con un calendario.
En este proyecto sólo se interactua con un calendario de Outlook, pero si en un futuro se quiere utilizar un calendario de Google por ejemplo, se debería crear la clase que implemente esta interfaz.

#### OutlookCalendarService
Esta clase es la implementaciñon específica de Outlook, usa un mapper (OutlookCalendarEventMapper) para poder convertir eventos de Outlook a los dtos que se manejan internamente en la API y viceversa. 

## Frameworks
El proyecto lo hice en un principio usando .Net 6, pero como era un requisito estricto usar .Net 5 realicé la migración. Igualmente la implementación para .Net 6 se encuentra en el siguiente branch: net-6-version

## Documentacion
https://docs.microsoft.com/es-es/graph/api/resources/event?view=graph-rest-1.0

https://docs.microsoft.com/es-es/azure/active-directory/develop/scenario-web-api-call-api-app-configuration?tabs=aspnetcore

https://docs.microsoft.com/es-es/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow



