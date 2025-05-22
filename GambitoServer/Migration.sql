CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TYPE tipo_hora AS ENUM ('banco_horas', 'hora_extra');

CREATE TABLE defeito (
    id integer GENERATED ALWAYS AS IDENTITY,
    nome character varying(50) NOT NULL,
    CONSTRAINT defeito_pkey PRIMARY KEY (id)
);

CREATE TABLE etapa (
    id integer GENERATED ALWAYS AS IDENTITY,
    nome character varying(100) NOT NULL,
    CONSTRAINT etapa_pkey PRIMARY KEY (id)
);

CREATE TABLE funcao (
    id integer GENERATED ALWAYS AS IDENTITY,
    nome character varying(100) NOT NULL,
    CONSTRAINT funcao_pkey PRIMARY KEY (id)
);

CREATE TABLE linha_producao (
    id integer GENERATED ALWAYS AS IDENTITY,
    descricao text,
    CONSTRAINT linha_producao_pkey PRIMARY KEY (id)
);

CREATE TABLE produto (
    id integer GENERATED ALWAYS AS IDENTITY,
    nome character varying(100) NOT NULL,
    tempo_peca integer NOT NULL,
    CONSTRAINT produto_pkey PRIMARY KEY (id)
);

CREATE TABLE funcionario (
    id integer GENERATED ALWAYS AS IDENTITY,
    nome character varying(100) NOT NULL,
    funcao integer,
    encarregado integer,
    invativo boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT funcionario_pkey PRIMARY KEY (id),
    CONSTRAINT funcionario_encarregado_fkey FOREIGN KEY (encarregado) REFERENCES funcionario (id),
    CONSTRAINT funcionario_funcao_fkey FOREIGN KEY (funcao) REFERENCES funcao (id)
);

CREATE TABLE linha_producao_dia (
    linha_producao integer NOT NULL,
    data date NOT NULL,
    invativo boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT linha_producao_dia_pkey PRIMARY KEY (linha_producao, data),
    CONSTRAINT linha_producao_dia_linha_producao_fkey FOREIGN KEY (linha_producao) REFERENCES linha_producao (id)
);

CREATE TABLE pedido (
    id integer GENERATED ALWAYS AS IDENTITY,
    produto integer NOT NULL,
    qtd_pecas integer NOT NULL,
    CONSTRAINT pedido_pkey PRIMARY KEY (id),
    CONSTRAINT pedido_produto_fkey FOREIGN KEY (produto) REFERENCES produto (id)
);

CREATE TABLE linha_producao_hora (
    id integer GENERATED ALWAYS AS IDENTITY,
    linha_producao integer NOT NULL,
    data date,
    hora time without time zone NOT NULL,
    pedido integer NOT NULL,
    qtd_produzido integer,
    paralizacao boolean NOT NULL DEFAULT FALSE,
    hora_ini time without time zone,
    hora_fim time without time zone,
    tipo tipo_hora,
    CONSTRAINT linha_producao_hora_pkey PRIMARY KEY (id),
    CONSTRAINT linha_producao_hora_linha_producao_data_fkey FOREIGN KEY (linha_producao, data) REFERENCES linha_producao_dia (linha_producao, data),
    CONSTRAINT linha_producao_hora_pedido_fkey FOREIGN KEY (pedido) REFERENCES pedido (id)
);

CREATE TABLE linha_producao_hora_defeito (
    linha_producao_hora integer NOT NULL,
    retrabalhado boolean NOT NULL,
    defeito integer NOT NULL,
    qtd_pecas integer NOT NULL,
    CONSTRAINT linha_producao_hora_defeito_pkey PRIMARY KEY (linha_producao_hora, retrabalhado, defeito),
    CONSTRAINT linha_producao_hora_defeito_defeito_fkey FOREIGN KEY (defeito) REFERENCES defeito (id),
    CONSTRAINT linha_producao_hora_defeito_linha_producao_hora_fkey FOREIGN KEY (linha_producao_hora) REFERENCES linha_producao_hora (id)
);

CREATE TABLE linha_producao_hora_etapa (
    id integer GENERATED ALWAYS AS IDENTITY,
    linha_producao_hora integer NOT NULL,
    etapa integer,
    ordem integer NOT NULL,
    segundos integer NOT NULL,
    CONSTRAINT linha_producao_hora_etapa_pkey PRIMARY KEY (id),
    CONSTRAINT linha_producao_hora_etapa_etapa_fkey FOREIGN KEY (etapa) REFERENCES etapa (id),
    CONSTRAINT linha_producao_hora_etapa_linha_producao_hora_fkey FOREIGN KEY (linha_producao_hora) REFERENCES linha_producao_hora (id) ON DELETE CASCADE
);

CREATE TABLE linha_producao_hora_etapa_funcionario (
    linha_producao_hora_etapa integer NOT NULL,
    funcionario integer NOT NULL,
    CONSTRAINT linha_producao_hora_etapa_funcionario_pkey PRIMARY KEY (linha_producao_hora_etapa, funcionario),
    CONSTRAINT linha_producao_hora_etapa_funcio_linha_producao_hora_etapa_fkey FOREIGN KEY (linha_producao_hora_etapa) REFERENCES linha_producao_hora_etapa (id),
    CONSTRAINT linha_producao_hora_etapa_funcionario_funcionario_fkey FOREIGN KEY (funcionario) REFERENCES funcionario (id)
);

CREATE UNIQUE INDEX ix_defeito_nome ON defeito (nome);

CREATE UNIQUE INDEX ix_funcao_nome ON funcao (nome);

CREATE INDEX ix_funcionario_encarregado ON funcionario (encarregado);

CREATE INDEX ix_funcionario_funcao ON funcionario (funcao);

CREATE INDEX ix_linha_producao_hora_linha_producao_data ON linha_producao_hora (linha_producao, data);

CREATE INDEX ix_linha_producao_hora_pedido ON linha_producao_hora (pedido);

CREATE INDEX ix_linha_producao_hora_defeito_defeito ON linha_producao_hora_defeito (defeito);

CREATE INDEX ix_linha_producao_hora_etapa_etapa ON linha_producao_hora_etapa (etapa);

CREATE INDEX ix_linha_producao_hora_etapa_linha_producao_hora ON linha_producao_hora_etapa (linha_producao_hora);

CREATE INDEX ix_linha_producao_hora_etapa_funcionario_funcionario ON linha_producao_hora_etapa_funcionario (funcionario);

CREATE INDEX ix_pedido_produto ON pedido (produto);

CREATE UNIQUE INDEX ix_produto_nome ON produto (nome);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250522200033_Init', '9.0.5');

ALTER TABLE linha_producao_hora ALTER COLUMN hora_ini TYPE time;

ALTER TABLE linha_producao_hora ALTER COLUMN hora_fim TYPE time;

ALTER TABLE linha_producao_hora ALTER COLUMN hora TYPE time;

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250522203416_Noda', '9.0.5');

COMMIT;

