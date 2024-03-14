-- Database: jdgpostgres

-- DROP DATABASE IF EXISTS jdgpostgres;

-- CREATE DATABASE jdgpostgres
--     WITH
--     OWNER = jdg
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'en_US.utf8'
--     LC_CTYPE = 'en_US.utf8'
--     LOCALE_PROVIDER = 'libc'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1
--     IS_TEMPLATE = False;



-- Table: public.student

-- DROP TABLE IF EXISTS public.student;

CREATE TABLE IF NOT EXISTS public.student
(
    id integer NOT NULL,
    name character varying COLLATE pg_catalog."default",
    address character varying COLLATE pg_catalog."default",
    phonenumber1 character varying COLLATE pg_catalog."default",
    phonenumber2 character varying COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.student
    OWNER to jdg;