create table public.jobs
(
    id         uuid                     not null
        constraint jobs_pk
            primary key,
    state      integer                  not null,
    created_at timestamp with time zone not null,
    base_type  varchar,
    method     varchar,
    arguments  varchar
);