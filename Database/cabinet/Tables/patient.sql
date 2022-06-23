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