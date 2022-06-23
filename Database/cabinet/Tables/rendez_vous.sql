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