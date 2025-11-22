CREATE TABLE Asignaturas (
    IdAsignatura INT IDENTITY(1,1) PRIMARY KEY,
    NombreAsignatura VARCHAR(100) NOT NULL,
    IdNivel INT NOT NULL FOREIGN KEY REFERENCES NivelesEducativos(IdNivel),
    IdEspecializacion INT NULL FOREIGN KEY REFERENCES Especializaciones(IdEspecializacion),
    UNIQUE (NombreAsignatura, IdNivel, IdEspecializacion)
);
