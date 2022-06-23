create table admin
(
    id       bigint       not null identity(1,1)
        primary key,
    email    varchar(255) null,
    password varchar(255) null,
    role     varchar(255) null
);