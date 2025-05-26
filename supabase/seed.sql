
-- Inserindo dados na tabela 'organizacao'
INSERT INTO organizacao (nome) VALUES
('Fábrica de Parafusos S.A.'),
('Metalúrgica União Ltda.');

WITH user_values AS
  (SELECT
    uuid_generate_v4() AS id,
    '00000000-0000-0000-0000-000000000000'::uuid AS instance_id,
    'authenticated' AS aud,
    'authenticated' AS role,
    (ROW_NUMBER() OVER ()) || '@gmail.com' AS email,
    crypt('password123', gen_salt('bf')) AS encrypted_password,
    now() AS email_confirmed_at,
    NULL::timestamp AS invited_at,
    '' AS confirmation_token,
    NULL::timestamp AS confirmation_sent_at,
    '' AS recovery_token,
    NULL::timestamp AS recovery_sent_at,
    '' AS email_change_token_new,
    '' AS email_change,
    NULL::timestamp AS email_change_sent_at,
    now()::timestamp AS last_sign_in_at,
    '{"provider":"email","providers":["email"]}'::jsonb AS raw_app_meta_data,
    '{}'::jsonb AS raw_user_meta_data,
    0::boolean AS is_super_admin,
    '2022-10-04 03:41:27.391146+00'::timestamp AS created_at,
    '2022-10-04 03:41:27.391146+00'::timestamp AS updated_at,
    NULL AS phone,
    NULL::timestamp AS phone_confirmed_at,
    '' AS phone_change,
    '' AS phone_change_token,
    NULL::timestamp AS phone_change_sent_at,
    '' AS email_change_token_current,
    0 AS email_change_confirm_status,
    NULL::timestamp AS banned_until,
    '' AS reauthentication_token,
    NULL::timestamp AS reauthentication_sent_at FROM generate_series(1, 2)
  ),
inserted_users AS (
  INSERT INTO auth.users
    (id, instance_id, aud, role, email, encrypted_password, email_confirmed_at, invited_at, confirmation_token, confirmation_sent_at, recovery_token, recovery_sent_at, email_change_token_new, email_change, email_change_sent_at, last_sign_in_at, raw_app_meta_data, raw_user_meta_data, is_super_admin, created_at, updated_at, phone, phone_confirmed_at, phone_change, phone_change_token, phone_change_sent_at, email_change_token_current, email_change_confirm_status, banned_until, reauthentication_token, reauthentication_sent_at)
    SELECT * FROM user_values RETURNING id, instance_id)
INSERT
	INTO
	public.user_organizacao (
  usuario,
	organizacao
)
SELECT
	id,
	ROW_NUMBER() OVER ()
FROM
	user_values;

-- Inserindo dados na tabela 'funcao'
INSERT INTO funcao (organizacao, nome) VALUES
(1, 'Gerente de Produção'),
(1, 'Operador de Máquina'),
(1, 'Inspetor de Qualidade'),
(2, 'Supervisor de Linha'),
(2, 'Montador');

-- Inserindo dados na tabela 'funcionario'
-- Note que o encarregado (gerente) é inserido primeiro para que possa ser referenciado.
INSERT INTO funcionario (organizacao, nome, funcao, encarregado) VALUES
(1, 'Carlos Silva', 1, NULL), -- Gerente, sem encarregado
(1, 'Ana Pereira', 2, 1),
(1, 'João Santos', 2, 1),
(1, 'Maria Oliveira', 3, 1),
(2, 'Roberto Lima', 4, NULL), -- Supervisor, sem encarregado
(2, 'Fernanda Costa', 5, 5);

-- Inserindo dados na tabela 'produto'
INSERT INTO produto (organizacao, nome, tempo_peca) VALUES
(1, 'Parafuso Sextavado M8', 15),
(1, 'Porca Autotravante M6', 10),
(2, 'Chapa de Aço 2mm', 120),
(2, 'Suporte Metálico Tipo L', 180);

-- Inserindo dados na tabela 'etapa'
INSERT INTO etapa (organizacao, nome) VALUES
(1, 'Corte'),
(1, 'Rosqueamento'),
(1, 'Galvanização'),
(2, 'Dobra'),
(2, 'Solda'),
(2, 'Pintura');

-- Inserindo dados na tabela 'pedido'
INSERT INTO pedido (produto, qtd_pecas) VALUES
(1, 1000),
(2, 2500),
(3, 500),
(4, 750);

-- Inserindo dados na tabela 'defeito'
INSERT INTO defeito (organizacao, nome) VALUES
(1, 'Rosca espanada'),
(1, 'Falha na galvanização'),
(2, 'Solda fria'),
(2, 'Pintura com bolhas');

-- Inserindo dados na tabela 'linha_producao'
INSERT INTO linha_producao (organizacao, descricao) VALUES
(1, 'Linha de produção de parafusos'),
(1, 'Linha de produção de porcas'),
(2, 'Linha de montagem de suportes');

-- Inserindo dados na tabela 'linha_producao_dia'
INSERT INTO linha_producao_dia (linha_producao, data) VALUES
(1, '2025-05-26'),
(2, '2025-05-26'),
(3, '2025-05-27');

-- Inserindo dados na tabela 'linha_producao_hora'
INSERT INTO linha_producao_hora (linha_producao, data, hora, pedido, qtd_produzido, paralizacao, hora_ini, hora_fim, tipo) VALUES
(1, '2025-05-26', '08:00:00', 1, 240, FALSE, NULL, NULL, NULL),
(1, '2025-05-26', '09:00:00', 1, 235, FALSE, NULL, NULL, NULL),
(1, '2025-05-26', '10:00:00', 1, 0, TRUE, '10:15:00', '10:45:00', NULL), -- Exemplo de paralisação
(2, '2025-05-26', '08:00:00', 2, 360, FALSE, NULL, NULL, NULL),
(3, '2025-05-27', '18:00:00', 4, 30, FALSE, '18:00:00', '19:00:00', 'HORA_EXTRA'); -- Exemplo de hora extra

-- Inserindo dados na tabela 'linha_producao_hora_etapa'
INSERT INTO linha_producao_hora_etapa (linha_producao_hora, etapa, ordem, segundos) VALUES
(1, 1, 1, 5),  -- Hora 1, Etapa 'Corte'
(1, 2, 2, 10), -- Hora 1, Etapa 'Rosqueamento'
(2, 1, 1, 6),  -- Hora 2, Etapa 'Corte'
(2, 2, 2, 9),  -- Hora 2, Etapa 'Rosqueamento'
(4, 2, 1, 4),  -- Hora 4, Etapa 'Rosqueamento'
(4, 3, 2, 6);  -- Hora 4, Etapa 'Galvanização'

-- Inserindo dados na tabela 'linha_producao_hora_etapa_funcionario'
INSERT INTO linha_producao_hora_etapa_funcionario (linha_producao_hora_etapa, funcionario) VALUES
(1, 2), -- Etapa 1, Funcionário Ana
(2, 3), -- Etapa 2, Funcionário João
(3, 2), -- Etapa 3, Funcionário Ana
(4, 3), -- Etapa 4, Funcionário João
(5, 6), -- Etapa 5, Funcionário Fernanda
(6, 6); -- Etapa 6, Funcionário Fernanda

-- Inserindo dados na tabela 'linha_producao_hora_defeito'
INSERT INTO linha_producao_hora_defeito (linha_producao_hora, retrabalhado, defeito, qtd_pecas) VALUES
(1, TRUE, 1, 5),   -- Hora 1, retrabalhado, 'Rosca espanada', 5 peças
(2, FALSE, 1, 2),  -- Hora 2, não retrabalhado, 'Rosca espanada', 2 peças
(4, TRUE, 2, 10);  -- Hora 4, retrabalhado, 'Falha na galvanização', 10 peças
