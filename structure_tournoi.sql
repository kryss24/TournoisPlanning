-- Active: 1744998930745@@127.0.0.1@3306@tournoismanagement

-- Table Tournois
CREATE TABLE Tournois (
    id INT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Type VARCHAR(50),
    Jeux VARCHAR(100)
);

-- Table Saison (plusieurs saisons pour un tournoi)
CREATE TABLE Saison (
    id INT PRIMARY KEY,
    date_debut DATE NOT NULL,
    date_fin DATE NOT NULL,
    id_tournoi INT NOT NULL,
    FOREIGN KEY (id_tournoi) REFERENCES Tournois(id) ON DELETE CASCADE
);

-- Table Equipe
CREATE TABLE Equipe (
    id INT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL
);

-- Table Joueur (plusieurs joueurs dans une équipe, un joueur dans une seule équipe)
CREATE TABLE Joueur (
    id INT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Poste VARCHAR(50),
    id_equipe INT NOT NULL,
    FOREIGN KEY (id_equipe) REFERENCES Equipe(id) ON DELETE CASCADE
);

-- Table Match (chaque match dans une saison, avec 2 équipes)
CREATE TABLE Matche (
    id INT PRIMARY KEY,
    Score VARCHAR(50),
    date DATE NOT NULL,
    id_saison INT NOT NULL,
    id_equipe1 INT NOT NULL,
    id_equipe2 INT NOT NULL,
    FOREIGN KEY (id_saison) REFERENCES Saison(id) ON DELETE CASCADE,
    FOREIGN KEY (id_equipe1) REFERENCES Equipe(id),
    FOREIGN KEY (id_equipe2) REFERENCES Equipe(id),
    CHECK (id_equipe1 <> id_equipe2)
);
