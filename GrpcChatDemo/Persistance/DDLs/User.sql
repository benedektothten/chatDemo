CREATE TABLE chat."user" (
                             id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
                             name varchar NULL,
                             "password" varchar NULL,
                             CONSTRAINT user_pk PRIMARY KEY (id)
);