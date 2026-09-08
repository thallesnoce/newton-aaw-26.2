# Participação em Aula — Arquitetura de Aplicações Web · 2026.2

**Disciplina:** Arquitetura de Aplicações Web
**Semestre:** 2026.2
**Horário:** Quintas-feiras, 18h55 às 22h30
**Professor:** Thalles Noce

---

## 1. Como funciona

A participação é acompanhada pelo **histórico de commits do seu repositório no GitHub**. Cada aula normalmente contém **atividade de fixação** e/ou **aula prática** — ambas devem estar commitadas na pasta correta até o prazo correspondente.

O que vale como participação:

- Atividades de fixação (exercícios curtos feitos durante a explicação)
- Aulas práticas (exercícios maiores, hands-on)
- Qualquer material produzido em aula que demonstre engajamento com o conteúdo

> 💡 **Não basta estar presente em sala.** A participação é evidenciada pelo commit no repositório, não pela presença física.

---

## 2. Estrutura obrigatória

Cada aula deve ter **sua própria pasta** dentro de `aaw/aulas/`, seguindo a nomeação com dois dígitos:

```
ra-123456/
└── aaw/
    └── aulas/
        ├── aula-01/    ← seus arquivos da aula 01
        ├── aula-02/    ← seus arquivos da aula 02
        ├── aula-03/
        └── ...
```

### Regras de nomeação

| ✅ Correto | ❌ Errado |
|---|---|
| `aaw/aulas/aula-01/` | `aulas/aula-01/` (falta o `aaw/`) |
| `aaw/aulas/aula-02/` | `aaw/aula-02/` (falta o `aulas/`) |
| `aaw/aulas/aula-10/` | `aaw/aulas/aula10/` (falta o hífen) |
| `aaw/aulas/aula-03/` | `aaw/aulas/Aula-03/` (maiúscula) |
| `aaw/aulas/aula-05/` | `aaw/aulas/aula-5/` (falta o zero) |

> 🚫 **Estrutura errada não conta.** Se os arquivos estiverem fora da pasta esperada ou com nomeação diferente, a atividade é considerada **não entregue**.

---

## 3. Prazos de entrega

### Regra geral

Cada atividade deve ser commitada e pushada em até **2 (dois) dias corridos** contados a partir da data da aula.

> **Exemplo:** a Aula 05 acontece na quinta-feira 03/09 → o commit deve estar no GitHub até **sábado 05/09, 23h59**.

### Exceção — Aulas 01 a 04

Como as primeiras semanas são de adaptação ao fluxo de trabalho com Git, as aulas 01, 02, 03 e 04 têm **prazo estendido**: todas podem ser entregues até **10/09/2026 (quarta-feira), 23h59**.

---

## 4. Tabela de prazos — AAW 2026.2

| Aula | Data da aula | Conteúdo | Pasta esperada | Prazo do commit |
|:---:|:---:|---|---|:---:|
| 01 | 06/08 (qui) | Do Monolito aos Microsserviços | `aaw/aulas/aula-01/` | **10/09** ⚠️ |
| 02 | 13/08 (qui) | REST: Fundamentos | `aaw/aulas/aula-02/` | **10/09** ⚠️ |
| 03 | 20/08 (qui) | REST: Design de APIs | `aaw/aulas/aula-03/` | **10/09** ⚠️ |
| 04 | 27/08 (qui) | Persistência Distribuída | `aaw/aulas/aula-04/` | **10/09** ⚠️ |
| 05 | 03/09 (qui) | Comunicação entre Serviços | `aaw/aulas/aula-05/` | 05/09 |
| 06 | 10/09 (qui) | Segurança em Aplicações Web (OWASP) | `aaw/aulas/aula-06/` | 12/09 |
| 07 | 17/09 (qui) | Autenticação e Autorização | `aaw/aulas/aula-07/` | 19/09 |
| 08 | 08/10 (qui) | RBAC e Controle de Acesso | `aaw/aulas/aula-08/` | 10/10 |
| 09 | 15/10 (qui) | Documentação com OpenAPI | `aaw/aulas/aula-09/` | 17/10 |
| 10 | 22/10 (qui) | Testes e Qualidade | `aaw/aulas/aula-10/` | 24/10 |
| 11 | 29/10 (qui) | Resiliência e Carga | `aaw/aulas/aula-11/` | 31/10 |
| 12 | 05/11 (qui) | Observabilidade | `aaw/aulas/aula-12/` | 07/11 |
| 13 | 12/11 (qui) | Conteinerização | `aaw/aulas/aula-13/` | 14/11 |

> ⚠️ **Prazo de exceção:** aulas 01 a 04 podem ser entregues até 10/09/2026, 23h59.
>
> Todos os demais prazos vencem às **23h59** do dia indicado.

---

## 5. O que é verificado

O professor verifica três coisas para cada aula:

| # | Critério | Como é conferido |
|:---:|---|---|
| 1 | O **commit existe** antes do prazo | `git log --before="<prazo> 23:59"` |
| 2 | O conteúdo está na **pasta correta** | Navegação pelo repositório no GitHub |
| 3 | A pasta contém **arquivos reais** da atividade | Inspeção do conteúdo (não vale README vazio ou placeholder) |

### O que NÃO conta como entrega

- 🚫 Commit feito **após o prazo** — mesmo que por minutos.
- 🚫 Arquivos na **pasta errada** ou com nomeação diferente da seção 2.
- 🚫 Pasta contendo **apenas um README.md vazio** ou arquivo placeholder.
- 🚫 Código **copiado integralmente** de outro aluno (verificado por comparação entre repositórios).
- 🚫 Um **único commit gigante** com todas as aulas de uma vez — o histórico deve refletir progresso incremental.

> ⚠️ **Não há reabertura de prazo** para atividades de aula. Planeje-se para fazer o commit no dia da aula ou, no máximo, nos dois dias seguintes.

---

## 6. Fluxo de trabalho recomendado

### Durante a aula

```bash
# 1. Puxe atualizações antes de começar
git pull

# 2. Crie a pasta da aula do dia
mkdir aaw/aulas/aula-05

# 3. Trabalhe nos exercícios dentro dessa pasta
# ... (crie arquivos, escreva código, etc.)

# 4. Ao final da aula, envie seu trabalho
git add .
git commit -m "AAW - Aula 05 - comunicação entre serviços"
git push
```

### Após a aula (dentro do prazo de 2 dias)

Se não terminou em aula, finalize em casa e faça o push **antes do prazo**:

```bash
# Finalize os exercícios na pasta da aula
git add .
git commit -m "AAW - Aula 05 - finaliza exercícios"
git push
```

### Como conferir se seu commit está no prazo

```bash
# Veja a data e hora do seu último commit
git log -1 --format="%ci" -- aaw/aulas/aula-05/

# Veja como o professor vai verificar (exemplo: prazo 05/09)
git log --before="2026-09-05 23:59" -- aaw/aulas/aula-05/
```

Se o comando acima retornar vazio, **seu commit não está no prazo**.

---

## 7. Resumo das regras

| Regra | Detalhe |
|---|---|
| **Prazo padrão** | 2 dias corridos após a aula, 23h59 |
| **Exceção** | Aulas 01–04 → até 10/09/2026 |
| **Estrutura** | `aaw/aulas/aula-XX/` (XX com dois dígitos) |
| **Evidência** | Commit no GitHub com arquivos reais |
| **Atraso** | Não aceito — sem reabertura de prazo |
| **Histórico** | Deve ser incremental, não um dump único |
