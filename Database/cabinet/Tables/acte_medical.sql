create table acte_medical
(
    id            bigint       not null identity(1,1)
        primary key ,
    libelle       varchar(255) null,
    specialite_id bigint       null,
    constraint FK8brb14928onw358u2x3xcgxgm
        foreign key (specialite_id) references specialite (id)
);