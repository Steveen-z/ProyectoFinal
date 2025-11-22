CREATE TABLE Notas (
    IdNota INT IDENTITY(1,1) PRIMARY KEY,
    IdEstudiante INT NOT NULL FOREIGN KEY REFERENCES Estudiantes(IdEstudiante),
    IdAsignatura INT NOT NULL FOREIGN KEY REFERENCES Asignaturas(IdAsignatura),
    Periodo VARCHAR(20) NOT NULL,
    Calificacion DECIMAL(5, 2) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    UNIQUE (IdEstudiante, IdAsignatura, Periodo)
);
