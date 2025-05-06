-- create a table
CREATE TABLE linha_producao (
  id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
  descricao TEXT,
  data DATE,
  produto INT
);
