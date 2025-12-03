CREATE TABLE Docentes (
    IdDocente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    IdUsuario INT UNIQUE 
        FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    DUI VARCHAR(15) NULL,
    Telefono VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    CONSTRAINT UQ_Docentes_Email UNIQUE (Email)
);

