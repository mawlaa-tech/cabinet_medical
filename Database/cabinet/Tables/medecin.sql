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