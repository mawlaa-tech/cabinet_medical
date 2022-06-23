create table dossier_financier
(
    id          bigint       not null identity(1,1)
        primary key,
    num_dossier varchar(255) null,
    patient_id  bigint       not null unique,
    constraint FKn46sjyin2rm3o7onesf5ypye0
        foreign key (patient_id) references patient (id)
);