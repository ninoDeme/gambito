CREATE TABLE organizacao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(100) NOT NULL
);
ALTER TABLE organizacao ENABLE ROW LEVEL SECURITY;

CREATE TABLE user_organizacao (
  usuario UUID NOT NULL REFERENCES auth.users ON DELETE CASCADE,
  organizacao INT NOT NULL REFERENCES organizacao(id),
  primary key (usuario, organizacao)
);
ALTER TABLE user_organizacao ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver suas organizações disponiveis" ON user_organizacao
USING ( (SELECT auth.uid()) = usuario );

CREATE POLICY "Usuarios conseguem ver suas organizações" ON organizacao
USING ( id in ( select organizacao from user_organizacao ) );

CREATE TABLE funcao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  nome VARCHAR(100) NOT NULL,
  UNIQUE (organizacao, nome)
);
ALTER TABLE funcao ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver funções em suas organizações" ON funcao
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE funcionario (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  nome VARCHAR(100) NOT NULL,
  funcao INT REFERENCES funcao(id),
  encarregado INT REFERENCES funcionario(id),
  invativo BOOLEAN NOT NULL DEFAULT FALSE
);
ALTER TABLE funcionario ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver funcionarios em suas organizações" ON funcionario
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE produto (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  nome VARCHAR(100) NOT NULL UNIQUE,
  tempo_peca INT NOT NULL
);
ALTER TABLE produto ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver produtos em suas organizações" ON produto
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE etapa (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  nome VARCHAR(100) NOT NULL,
  UNIQUE (organizacao, nome)
);
ALTER TABLE etapa ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver etapas em suas organizações" ON etapa
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE pedido (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  produto INT NOT NULL REFERENCES produto(id),
  descricao VARCHAR(256),
  qtd_pecas INT NOT NULL
);
ALTER TABLE pedido ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver pedidos em suas organizações" ON pedido
USING ( produto in ( select produto from produto ) );

CREATE TABLE defeito (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  nome VARCHAR(50) NOT NULL UNIQUE
);
ALTER TABLE defeito ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver defeitos em suas organizações" ON defeito
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE linha_producao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  organizacao INT REFERENCES organizacao(id),
  descricao TEXT
);
ALTER TABLE linha_producao ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao
USING ( organizacao in ( select organizacao from user_organizacao ) );

CREATE TABLE linha_producao_dia (
  linha_producao INT NOT NULL REFERENCES linha_producao(id),
  data DATE,
  invativo BOOLEAN NOT NULL DEFAULT FALSE,
  PRIMARY KEY (linha_producao, data)
);
ALTER TABLE linha_producao_dia ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao_dia
USING ( linha_producao in ( select id from linha_producao ) );

CREATE TYPE tipo_hora AS ENUM ('HORA_EXTRA', 'BANCO_HORAS');

CREATE TABLE linha_producao_hora (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  linha_producao INT NOT NULL,
  data DATE,
  hora TIME NOT NULL,
  pedido INT NOT NULL REFERENCES pedido(id),
  qtd_produzido INT,
  paralizacao BOOLEAN NOT NULL DEFAULT FALSE,
  hora_ini TIME,
  hora_fim TIME,
  tipo TIPO_HORA,
  FOREIGN KEY(linha_producao, data) REFERENCES linha_producao_dia(linha_producao, data)
);
ALTER TABLE linha_producao_hora ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao_hora
USING ( linha_producao in ( select id from linha_producao ) );

CREATE TABLE linha_producao_hora_etapa (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  linha_producao_hora INT NOT NULL,
  etapa INT references etapa(id),
  ordem INT NOT NULL,
  segundos INT NOT NULL,
  FOREIGN KEY(linha_producao_hora) REFERENCES linha_producao_hora(id),
  UNIQUE (linha_producao_hora, etapa)
);
ALTER TABLE linha_producao_hora_etapa ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao_hora_etapa
USING ( linha_producao_hora in ( select id from linha_producao_hora ) );


CREATE TABLE linha_producao_hora_etapa_funcionario (
  linha_producao_hora_etapa INT NOT NULL REFERENCES linha_producao_hora_etapa(id),
  funcionario INT NOT NULL REFERENCES funcionario(id),
  PRIMARY KEY(linha_producao_hora_etapa, funcionario)
);
ALTER TABLE linha_producao_hora_etapa_funcionario ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao_hora_etapa_funcionario
USING ( funcionario in ( select id from funcionario ) );

CREATE TABLE linha_producao_hora_defeito (
  linha_producao_hora INT NOT NULL REFERENCES linha_producao_hora(id),
  retrabalhado BOOLEAN NOT NULL,
  defeito INT NOT NULL REFERENCES defeito(id),
  qtd_pecas INT NOT NULL,
  PRIMARY KEY (linha_producao_hora, retrabalhado, defeito)
);
ALTER TABLE linha_producao_hora_defeito ENABLE ROW LEVEL SECURITY;

CREATE POLICY "Usuarios conseguem ver linhas de produção em suas organizações" ON linha_producao_hora_defeito
USING ( defeito in ( select id from defeito ) );
