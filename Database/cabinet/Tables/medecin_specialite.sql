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