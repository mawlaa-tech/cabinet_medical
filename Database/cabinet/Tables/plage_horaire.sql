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