
INSERT INTO dbo.admin (email, password, role) VALUES ('karineattolou@gmail.com', '$2a$10$bA6VISe5tAqi0v4mtKiH3uM2R76YsAUZx6VYaEz.o1jW2KNFw6bx.', 'ROLE_ADMIN');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Allergie et immunologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Anesthesiologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Chirurgie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Dermatologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Genetique');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Gynecologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Medecine familliale');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Medecine interne');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Medecine preventive');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Medecine physique et rehabilitation');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Medecine d''urgence');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Neurologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Ophtalmologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Oncologie nucleaire');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Pathologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Pediatrie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Psychiatrie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Radiologie');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Radiologie nucleaire');
GO

INSERT INTO dbo.specialite (nom) VALUES ('Urologie');
GO

INSERT INTO dbo.acte_medical (libelle, specialite_id) VALUES ('Appendicectomie', 3);
GO

INSERT INTO dbo.acte_medical (libelle, specialite_id) VALUES ('Opération de la cataracte', 3);
GO

INSERT INTO dbo.acte_medical (libelle, specialite_id) VALUES ('Pontage coronarien', 3);
GO

INSERT INTO dbo.acte_medical (libelle, specialite_id) VALUES ('Chirurgie de reconstruction du LCA', 3);
GO

INSERT INTO dbo.medecin (email, nom_medecin, password, prenom_medecin, role) VALUES ('belloabdoul@gmail.com', 'BELLO', '$2a$10$6GQQNe1Tl9LGfk5hQwiXju32VwrYAKbP66GXd5upQ91skB4dAcER.', 'Abdoul Koudous', 'ROLE_MEDECIN');
GO

INSERT INTO dbo.medecin (email, nom_medecin, password, prenom_medecin, role) VALUES ('med_amghar@hotmail.com', 'AMGHAR', '$2a$10$wB7s52h/PwgEOrH9BqxsWeUvbNNhGrFxJMz0WM1SRQ1FUhqW5vm/y', 'Mohamed Amine', 'ROLE_MEDECIN');
GO

INSERT INTO dbo.medecin (email, nom_medecin, password, prenom_medecin, role) VALUES ('SalimouDrame@gmail.com', 'DRAME', '$2a$10$4Hy2PzE6RK34Gz4Z1PYLI.9edbSdwkHOhV0FiVGZdcZMC/9viKfw2', 'Salimou', 'ROLE_MEDECIN');
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (1, 3);
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (3, 3);
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (2, 7);
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (2, 9);
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (3, 11);
GO

INSERT INTO dbo.medecin_specialite (medecin_id, specialite_id) VALUES (3, 18);
GO
