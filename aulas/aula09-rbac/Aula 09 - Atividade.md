# HANDOUT — AULA 09

## Projete a Matriz de Acesso

*SisAcad — sistema acadêmico fictício — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são o time de segurança contratado para definir QUEM pode fazer O QUÊ no SisAcad, o novo sistema acadêmico da faculdade. Os papéis já existem: Aluno, Professor, Coordenador e Secretaria.

- Preencham a matriz abaixo com ✓ (pode) ou ✗ (não pode) para cada papel × ação
- Justifiquem por escrito os 3 casos que geraram mais discussão na dupla
- Respondam à pergunta-armadilha da Parte 03 — ela esconde o limite do modelo

*⏱️ Tempo: 25 minutos  |  👥 Formato: em duplas  |  Regra de ouro: na dúvida, NEGUE o acesso.*

> **Nomes:** ____________________   **Turma:** ____________________   **Data:** ___ / ___ / ______

## PARTE 01 — A matriz papel × permissão

Marquem ✓ (pode) ou ✗ (não pode) em cada célula:

| **Ação** | **Aluno** | **Professor** | **Coordenador** | **Secretaria** |
| --- | --- | --- | --- | --- |
| 1. Ver as próprias notas |  |  |  |  |
| 2. Ver notas de toda a turma |  |  |  |  |
| 3. Lançar nota |  |  |  |  |
| 4. Alterar nota já lançada |  |  |  |  |
| 5. Matricular aluno em disciplina |  |  |  |  |
| 6. Trancar matrícula |  |  |  |  |
| 7. Criar nova disciplina |  |  |  |  |
| 8. Ver dados pessoais de aluno |  |  |  |  |
| 9. Emitir histórico escolar |  |  |  |  |
| 10. Aprovar colação de grau |  |  |  |  |

*Dica: pensem em consequências — quem responde perante o MEC se uma nota for alterada indevidamente? E perante a LGPD se dados pessoais vazarem?*

## PARTE 02 — Os casos polêmicos

Quais 3 células geraram mais discussão na dupla? Anotem a célula (ação + papel) e a justificativa da decisão final:

1. Caso 1:

2. Caso 2:

3. Caso 3:

## PARTE 03 — A pergunta-armadilha

Vocês marcaram que Professor pode “lançar nota”. Mas… de QUALQUER turma? O professor de Cálculo pode lançar nota na turma de Arquitetura de Aplicações Web?

1. O papel “Professor” sozinho é suficiente para decidir esse acesso? Por quê?

2. Que informação ALÉM do papel o sistema precisa conhecer para decidir corretamente?

3. Onde vocês já viram esse mesmo problema nesta disciplina? (dica: Aula 07…)

## DESAFIO

1. Chegou um pedido da coordenação: criar o papel “Monitor” — um aluno que ajuda o professor em uma disciplina. Que permissões vocês dariam ao Monitor? Que risco novo esse papel cria?
