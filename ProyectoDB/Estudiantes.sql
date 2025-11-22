CREATE TABLE Estudiantes (
    IdEstudiante INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    IdNivel INT NOT NULL FOREIGN KEY REFERENCES NivelesEducativos(IdNivel),
    IdEspecializacion INT NULL FOREIGN KEY REFERENCES Especializaciones(IdEspecializacion),
    IdUsuario INT UNIQUE FOREIGN KEY REFERENCES Usuarios(IdUsuario)
);
