# OutlookCalendarRestApi
Api  REST que permite listar, crear, editar y eliminar eventos de un calendario de Outlook.

## Url OutlookCalendarRestApi Swagger
https://codacalendarapi.azurewebsites.net/swagger/index.html

## Autenticación 
La autenticación se realizó utilizando Microsoft Identity. Para poder ejecutar un endpoint se necesita un acces token, siguiendo el protocolo Oauth2. La forma más simple de probarlo es ingresar a swagger, presionar el botón "Authorize", seleccionar el scope requerido y luego loguearse con la cuenta de Microsoft cuyo calendario se quiera obtener. 
Una vez autenticado, retorna a swagger con un popup que indica que esta logueado, cerralo y ya se puede acceder a la API.

## Conexión con Microsoft Graph 
La conexion con Microsoft Graph se realizó utilizando el Flujo OBO (On behalf of). El request a la OutlookCalendarRestApi llega con un token otorgado para que el usuario para acceder. Este token es utilizado por la app para obtener un nuevo access token con los permisos necesarios para acceder a Graph. 
Mas información sobro el flujo OBO en: https://docs.microsoft.com/es-es/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow

Se configuró el cacheo de los tokens para el acceso a Graph en una base de datos Sql Server. 
La bd es coda_calendar_api y la tabla TokenCache.

## Endpoints
### GET /Authorization
Como este ejercicio se trataba de realizar una API, el flujo de delegación de permisos no se puede realizar completo y se necesita que el usuario haga algunas tareas manuales. Para eso se generó este endpoint el cual retorna una url para que el usuario pueda otorgar los permisos que requiere la api para acceder al calendario de Outlook. La url debe copiar y pegar en el browser. Se verá una página de autorización de permisos requeridos para leer y editar el calendario. Luego de aceptar, redireccionará a una url que no existe (redirect url), lo cual no es un inconveniente ya que estamos usando los endpoints de la API directamente.
Este flujo es mucho mas claro cuando se tiene un sitio web que hace de intermediario entre la API y el identity provider.
Ya con los permisos otorgados, se pueden probar el resto de endpoints.

### GET /Calendar
Retorna los eventos del calendario del usuario logueado, ordenados por fecha en forma descendente. 
(Nota: se retornan los primeros 50, no se realizó paginado por cuestion de tiempos).

### POST /Calendar
Crea un nuevo evento en el calendario.

### PATCH /Calendar
Dado el id de un evento existente, actualiza las prpiedades de un evento.

### DELETE /Calendar
Dado el id de un evento existente, lo elimina del calendario.

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
Para las solicitudes al calendario.

### Modelo

#### EventDto
Dto que representa los conceptos básicos de un evento de calendario. Se intentó abstraer el concepto del dominio de la implementación de Outlook.

## Servicios
#### ICalendarService
Interfaz que representa la comunicación con un calendario.
En este proyecto sólo se interactua con un calendario de Outlook, pero si en un futuro se quiere utilizar un el calendario de Google por ejemplo, se debería crear la clase que implemente esta interfaz.

#### OutlookCalendarService
Esta clase es la implementaciñon especifica de Outlook, usa un mapper (OutlookCalendarEventMapper) para poder convertir eventos de Outlook a los dtos que se manejan internamente en la api y viceversa. El mapper es una clase totalmente ligada al proveedor del calendario. 

## Frameworks
El proyecto lo hice en un principio usando .Net 6, pero como era un requisito estricto usar .Net 5 realicé la migración. Igualmente la implementación para .Net 6 se encuentra en el siguiente branch: 

## Documentacion
https://docs.microsoft.com/es-es/graph/api/resources/event?view=graph-rest-1.0

https://docs.microsoft.com/es-es/azure/active-directory/develop/scenario-web-api-call-api-app-configuration?tabs=aspnetcore

https://docs.microsoft.com/es-es/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow



