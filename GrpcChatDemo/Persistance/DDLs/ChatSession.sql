CREATE TABLE chat.chatsession (
                                  id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
                                  "name" varchar NULL,
                                  owner_id int4 NOT NULL,
                                  CONSTRAINT chatsession_pk PRIMARY KEY (id),
                                  CONSTRAINT chatsession_owner_fk FOREIGN KEY (owner_id) REFERENCES public."User"(id)
);