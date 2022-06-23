create table admin
(
    id       bigint       not null identity(1,1)
        primary key,
    email    varchar(255) null,
    password varchar(255) null,
    role     varchar(255) null
);

create table specialite
(
    id  bigint       not null identity(1,1)
        primary key,
    nom varchar(255) null
);

create table acte_medical
(
    id            bigint       not null identity(1,1)
        primary key ,
    libelle       varchar(255) null,
    specialite_id bigint       null,
    constraint FK8brb14928onw358u2x3xcgxgm
        foreign key (specialite_id) references specialite (id)
);

create table medecin
(
    id             bigint       not null identity(1,1)
        primary key,
    email          varchar(255) null,
    nom_medecin    varchar(255) null,
    password       varchar(255) null,
    prenom_medecin varchar(255) null,
    role           varchar(255) null
);

create table medecin_specialite
(
    medecin_id    bigint not null,
    specialite_id bigint not null,
    primary key (medecin_id, specialite_id),
    constraint FKect93bn1mx1eer9dygro2qqxl
        foreign key (specialite_id) references specialite (id),
    constraint FKorhfqpvf30ir3eqji1xper78w
        foreign key (medecin_id) references medecin (id)
);

create table patient
(
    id           bigint       not null identity(1,1)
        primary key,
    dat_nais_pat date         null,
    email        varchar(255) null,
    nom_pat      varchar(255) null,
    password     varchar(255) null,
    phone_pat    varchar(255) null,
    pren_pat     varchar(255) null,
    role         varchar(255) null,
    sex_pat      varchar(255) null
);

create table dossier_financier
(
    id          bigint       not null identity(1,1)
        primary key,
    num_dossier varchar(255) null,
    patient_id  bigint       not null unique,
    constraint FKn46sjyin2rm3o7onesf5ypye0
        foreign key (patient_id) references patient (id)
);

create table dossier_medical
(
    id          bigint       not null identity(1,1)
        primary key,
    num_dossier varchar(255) null,
    patient_id  bigint       not null unique,
    constraint FKhr7ar2qbk7e6j9l2s8ey8lerb
        foreign key (patient_id) references patient (id),
);

create table plage_horaire
(
    id              bigint not null identity(1,1)
        primary key,
    date            date   null,
    heure_debut     time   null,
    heure_ter       time   null,
    acte_medical_id bigint null,
    medecin_id      bigint null,
    specialite_id   bigint null,
    constraint FKaukbkcdfs9hmaof9eg9bmhor4
        foreign key (medecin_id) references medecin (id),
    constraint FKi44vbbd0f3lvqpwlxwn69skb9
        foreign key (acte_medical_id) references acte_medical (id),
    constraint FKje2feb755oyo88c9qekdnhtn2
        foreign key (specialite_id) references specialite (id)
);

create table rendez_vous
(
    id               bigint not null identity(1,1)
        primary key,
    patient_id       bigint null,
    plage_horaire_id bigint not null unique,
    constraint FK65icp6skpylqrx7x76kj8ukaw
        foreign key (plage_horaire_id) references plage_horaire (id),
    constraint FKjfqx91ipif2aimip15soh8kbm
        foreign key (patient_id) references patient (id)
);


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