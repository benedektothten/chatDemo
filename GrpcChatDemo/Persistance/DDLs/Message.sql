CREATE TABLE chat.message (
                              id int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
                              message varchar NULL,
                              "create" date NOT NULL,
                              user_id int4 NULL,
                              chat_session_id int4 NOT NULL,
                              CONSTRAINT message_pk PRIMARY KEY (id),
                              CONSTRAINT message_chatsession_fk FOREIGN KEY (chat_session_id) REFERENCES chat.chatsession(id),
                              CONSTRAINT message_user_fk FOREIGN KEY (user_id) REFERENCES public."User"(id)
);