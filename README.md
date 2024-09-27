
# EVALUACIÓN TÉCNICA NUXIBA #

Prueba: **DESARROLLADOR JR**

Deadline: **1 día**

Nombre: Eduardo Humberto Vázquez Fernández

------
## Clona y crea tu repositorio para la evaluación ##
* Clona este repositorio en tu máquina local
* Crear un repositorio público en tu cuenta personal de GitHub, BitBucket o Gitlab
* Cambia el origen remoto para que apunte al repositorio público que acabas crear en tu cuenta
* Coloca tu nombre en este archivo README.md y realiza un push al repositorio remoto

------

## Prueba 1 ##

Para esta prueba será necesario agregar (al directorio **Prueba_2**) las sentencias de SQL con las que se creo el esquema(base de datos y tablas) y las consultas realizadas para responder a los siguientes puntos:

> *Nota: Para realizar esta prueba es necesario tener una instancia de [SQL Server Developer, SQL Server Express](https://www.microsoft.com/es-mx/sql-server/sql-server-downloads) o [MySQL Community](https://dev.mysql.com/downloads/mysql/) instalada en tu equipo, si por algún motivo no puedes instalar una versión de las anteriores, puedes entrar a [db-fiddle](https://www.db-fiddle.com/) para realizarla*

* Construye una base de datos y crea las siguientes tablas dentro de ella:
	* **usuarios** con las siguientes columnas
		* **userId** int
		* **Login** varchar(100)
		* **Nombre** varchar(100)
		* **Paterno** varchar(100)
		* **Materno** varchar(100)

	* **empleados** esta debe tener relacion la tabla usuarios, con las siguientes columnas
		* **userId** int
		* **Sueldo** double
		* **FechaIngreso** date


		
* Con las tablas contruidas, inserta los datos proporcionados en el archivo de excel **DatosPracticaSQL.xlsx** que se encuentran en la carpeta **Prueba_1** (revisar las dos hojas del archivo)
* Genera las sentencias SQL que respondan lo siguiente:
	* Depurar solo los ID diferentes de 6,7,9 y 10  de la tabla  **usuarios** **_(5 puntos)_**
	* Actualizar el dato Sueldo en un 10 porciento a los empleados que tienen fechas entre el año 2000 y 2001 **_(5 puntos)_**
	* Realiza una consulta para traer el nombre de usuario y fecha de ingreso de los usuarios que gananen mas de 10000 y su apellido comience con 
	T ordernado del mas reciente al mas antiguo **_(10 puntos)_**
	* Realiza una consulta donde agrupes a los empleados por sueldo, un grupo con los que ganan menos de 1200 y uno mayor o igual a 1200, cuantos hay en cada grupo? **_(10 puntos)_**

------

## Prueba 2 ##
* Crea una aplicación (dentro del directorio **Prueba_2**) en .NET que realice lo siguiente:
	* Listar top 10 usuarios de la base antes creada **_(10 puntos)_**
	* Generar un archivo csv con las siguienets campos con su información(Login, Nombre completo, sueldo, fecha Ingreso)  **_(25 puntos)_**
	* Poder actualizar el salario del algun usuario especifico **_(10 puntos)_**
	* Poder Tener una opcion para agregar un nuevo usuario y se pueda asiganar el salario y la fecha de ingreso por default el dia de hoy **_(25 puntos)_**



**PLUS: Si conoces algún patrón de diseño de software no dudes en usarlo** **_(+ 10 puntos)_**

------

### Realiza el push del código de tus pruebas y compártenos el link a tu repositorio remoto 😊 

------
Si tienes alguna duda sobre la evaluación puedes mandar un correo electrónico a [desarrollo@nuxiba.com](mailto:desarrollo@nuxiba.com?subject=Dudas%20sobre%20evaluación%20técnica)

Manda la liga de tu repositorio público a [talento@nuxiba.com](mailto:talento@nuxiba.com?subject=[EvaluaciónDesarrollo]%20Este%20es%20mi%20repositorio)