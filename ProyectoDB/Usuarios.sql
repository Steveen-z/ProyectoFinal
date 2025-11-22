CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    -- Clave única para inicio de sesión (Código Estudiantil/Docente)
    CodigoAcceso VARCHAR(20) UNIQUE NOT NULL, 
    PasswordHash VARCHAR(256) NOT NULL,
    Rol VARCHAR(20) NOT NULL -- 'Admin', 'Docente', 'Estudiante'
);