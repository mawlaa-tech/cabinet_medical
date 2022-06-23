create table dossier_medical
(
    id          bigint       not null identity(1,1)
        primary key,
    num_dossier varchar(255) null,
    patient_id  bigint       not null unique,
    constraint FKhr7ar2qbk7e6j9l2s8ey8lerb
        foreign key (patient_id) references patient (id),
);