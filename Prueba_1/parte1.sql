CREATE DATABASE prueba_tecnica;
USE prueba_tecnica;
-- creación de tablas
CREATE TABLE usuarios (
    userId INT PRIMARY KEY AUTO_INCREMENT,
    Login VARCHAR(100),
    Nombre VARCHAR(100),
    Paterno VARCHAR(100),
    Materno VARCHAR(100)
);
CREATE TABLE empleados (
    userId INT,
    Sueldo DOUBLE,
    FechaIngreso DATE,
    FOREIGN KEY (userId) REFERENCES usuarios(userId)
);

-- aqui inserté los datos desde el programa Prueba_1\Test\Program.cs

-- Depurar solo los ID diferentes de 6,7,9 y 10 de la tabla usuarios (5 puntos)
DELETE FROM empleados
WHERE userId NOT IN (6, 7, 9, 10);

DELETE FROM usuarios 
WHERE userId NOT IN (6, 7, 9, 10);

-- Actualizar el dato Sueldo en un 10 porciento a los empleados que tienen fechas entre el año 2000 y 2001 (5 puntos)
UPDATE empleados 
SET Sueldo = Sueldo * 1.10 
WHERE FechaIngreso BETWEEN '2000-01-01' AND '2001-12-31';

-- Realiza una consulta para traer el nombre de usuario y fecha de ingreso de los usuarios que gananen mas de 10000 y su apellido comience con T ordernado del mas reciente al mas antiguo (10 puntos)
SELECT u.Nombre, u.Paterno, e.FechaIngreso 
FROM usuarios u 
JOIN empleados e ON u.userId = e.userId 
WHERE e.Sueldo > 10000 AND u.Paterno LIKE 'T%' 
ORDER BY e.FechaIngreso DESC;

-- Realiza una consulta donde agrupes a los empleados por sueldo, un grupo con los que ganan menos de 1200 y uno mayor o igual a 1200, cuantos hay en cada grupo? (10 puntos)
SELECT 
    CASE 
        WHEN Sueldo < 1200 THEN 'Menos de 1200' 
        ELSE '1200 o más' 
    END AS GrupoSueldo,
    COUNT(*) AS Cantidad
FROM empleados
GROUP BY GrupoSueldo;
