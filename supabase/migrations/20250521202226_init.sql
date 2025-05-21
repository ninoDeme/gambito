CREATE TABLE funcao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE funcionario (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(100) NOT NULL,
  funcao INT REFERENCES funcao(id),
  encarregado INT REFERENCES funcionario(id),
  invativo BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE produto (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(100) NOT NULL UNIQUE,
  tempo_peca INT NOT NULL
);

CREATE TABLE etapa (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(100) NOT NULL
);

CREATE TABLE pedido (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  produto INT NOT NULL REFERENCES produto(id),
  qtd_pecas INT NOT NULL
);

CREATE TABLE defeito (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  nome VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE linha_producao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  descricao TEXT
);

CREATE TABLE linha_producao_dia (
  linha_producao INT NOT NULL REFERENCES linha_producao(id),
  data DATE,
  invativo BOOLEAN NOT NULL DEFAULT FALSE,
  PRIMARY KEY (linha_producao, data)
);

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

CREATE TABLE linha_producao_hora_etapa (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  linha_producao INT NOT NULL,
  data DATE,
  etapa INT references etapa(id),
  ordem INT NOT NULL,
  segundos INT NOT NULL,
  FOREIGN KEY(linha_producao, data) REFERENCES linha_producao_dia(linha_producao, data)
);


CREATE TABLE linha_producao_hora_etapa_funcionario (
  linha_producao_hora_etapa INT NOT NULL REFERENCES linha_producao_hora_etapa(id),
  funcionario INT NOT NULL REFERENCES funcionario(id),
  PRIMARY KEY(linha_producao_hora_etapa, funcionario)
);

CREATE TABLE linha_producao_hora_defeito (
  linha_producao_hora INT NOT NULL REFERENCES linha_producao_hora(id),
  retrabalhado BOOLEAN NOT NULL,
  defeito INT NOT NULL REFERENCES defeito(id),
  qtd_pecas INT NOT NULL,
  PRIMARY KEY (linha_producao_hora, retrabalhado, defeito)
)
