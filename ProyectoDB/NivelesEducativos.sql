CREATE TABLE NivelesEducativos (
    IdNivel INT IDENTITY(1,1) PRIMARY KEY,
    TipoBachillerato VARCHAR(50) NOT NULL, -- 'General' o 'Técnico'
    Anio INT NOT NULL, -- 1, 2, 3
    CHECK (Anio IN (1, 2, 3)),
    UNIQUE (TipoBachillerato, Anio)
);