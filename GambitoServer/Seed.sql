-- Seed data for 'funcao' table
INSERT INTO funcao (nome) VALUES
('Operador de Máquina'),
('Montador'),
('Inspetor de Qualidade'),
('Supervisor de Produção'),
('Encarregado de Linha'),
('Mecânico de Manutenção');

---

-- Seed data for 'funcionario' table
INSERT INTO funcionario (nome, funcao, encarregado, invativo) VALUES
('João Silva', 4, NULL, FALSE), -- Supervisor de Produção
('Maria Oliveira', 5, 1, FALSE), -- Encarregado de Linha (reporta a João Silva)
('Pedro Santos', 1, 2, FALSE), -- Operador de Máquina (reporta a Maria Oliveira)
('Ana Costa', 1, 2, FALSE),   -- Operador de Máquina (reporta a Maria Oliveira)
('Carlos Souza', 2, 2, FALSE),  -- Montador (reporta a Maria Oliveira)
('Fernanda Lima', 3, 1, FALSE), -- Inspetor de Qualidade (reporta a João Silva)
('Mariana Rocha', 6, 1, FALSE), -- Mecânico de Manutenção (reporta a João Silva)
('Lucas Pereira', 1, 2, TRUE); -- Operador de Máquina (inativo)

---

-- Seed data for 'defeito' table
INSERT INTO defeito (nome) VALUES
('Rebarba'),
('Peça Trincada'),
('Desalinhamento'),
('Pintura Falha'),
('Componente Faltante'),
('Curto Circuito');

---

-- Seed data for 'etapa' table
INSERT INTO etapa (nome) VALUES
('Corte a Laser'),
('Dobragem'),
('Soldagem'),
('Pintura Eletrostática'),
('Montagem Principal'),
('Testes de Qualidade'),
('Embalagem');

---

-- Seed data for 'linha_producao' table
INSERT INTO linha_producao (descricao) VALUES
('Linha de Produção de Componentes Metálicos'),
('Linha de Montagem Final de Produtos Eletrônicos'),
('Linha de Fabricação de Peças Plásticas');

---

-- Seed data for 'produto' table
INSERT INTO produto (nome, tempo_peca) VALUES
('Componente X', 120), -- 2 minutos por peça
('Produto Y', 300),    -- 5 minutos por peça
('Peça Z', 60);      -- 1 minuto por peça

---

-- Seed data for 'pedido' table
INSERT INTO pedido (produto, qtd_pecas) VALUES
(1, 1000), -- Pedido de 1000 Componentes X
(2, 500),  -- Pedido de 500 Produtos Y
(3, 2000); -- Pedido de 2000 Peças Z

---

-- Seed data for 'linha_producao_dia' table
INSERT INTO linha_producao_dia (linha_producao, data, invativo) VALUES
(1, '2025-05-20', FALSE),
(2, '2025-05-20', FALSE),
(1, '2025-05-21', FALSE),
(3, '2025-05-21', FALSE);

---

-- Seed data for 'linha_producao_hora' table
INSERT INTO linha_producao_hora (linha_producao, data, hora, pedido, qtd_produzido, paralizacao, hora_ini, hora_fim, tipo) VALUES
(1, '2025-05-20', '08:00:00', 1, 30, FALSE, NULL, NULL, NULL),
(1, '2025-05-20', '09:00:00', 1, 25, TRUE, '09:00:00', '09:15:00', NULL), -- Paralização de 15 minutos
(2, '2025-05-20', '10:00:00', 2, 10, FALSE, NULL, NULL, NULL),
(1, '2025-05-21', '08:00:00', 1, 35, FALSE, NULL, NULL, NULL),
(1, '2025-05-21', '17:00:00', 1, 10, FALSE, '17:00:00', '18:00:00', 'hora_extra'), -- Hora extra
(3, '2025-05-21', '09:00:00', 3, 50, FALSE, NULL, NULL, NULL);

---

-- Seed data for 'linha_producao_hora_defeito' table
INSERT INTO linha_producao_hora_defeito (linha_producao_hora, retrabalhado, defeito, qtd_pecas) VALUES
(1, FALSE, 1, 2), -- 2 peças com rebarba (não retrabalhadas)
(1, TRUE, 1, 1),  -- 1 peça com rebarba (retrabalhada)
(2, FALSE, 2, 1), -- 1 peça trincada
(3, FALSE, 3, 1), -- 1 peça com desalinhamento
(4, FALSE, 1, 3); -- 3 peças com rebarba

---

-- Seed data for 'linha_producao_hora_etapa' table
INSERT INTO linha_producao_hora_etapa (linha_producao_hora, etapa, ordem, segundos) VALUES
(1, 1, 1, 60),  -- Linha 1, hora 1: Etapa 1 (Corte a Laser), 60 segundos
(1, 2, 2, 45),  -- Linha 1, hora 1: Etapa 2 (Dobragem), 45 segundos
(3, 5, 1, 180), -- Linha 2, hora 1: Etapa 5 (Montagem Principal), 180 segundos
(4, 1, 1, 65),  -- Linha 1, hora 2: Etapa 1 (Corte a Laser), 65 segundos
(6, 4, 1, 90);  -- Linha 3, hora 1: Etapa 4 (Pintura Eletrostática), 90 segundos

---

-- Seed data for 'linha_producao_hora_etapa_funcionario' table
INSERT INTO linha_producao_hora_etapa_funcionario (linha_producao_hora_etapa, funcionario) VALUES
(1, 3), -- Etapa 1 do Linha_Producao_Hora 1, Funcionário Pedro Santos
(2, 4), -- Etapa 2 do Linha_Producao_Hora 1, Funcionário Ana Costa
(3, 5), -- Etapa 5 do Linha_Producao_Hora 3, Funcionário Carlos Souza
(4, 3), -- Etapa 1 do Linha_Producao_Hora 4, Funcionário Pedro Santos
(5, 4); -- Etapa 4 do Linha_Producao_Hora 6, Funcionário Ana Costa
