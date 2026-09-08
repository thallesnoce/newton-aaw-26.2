# Criando seu repositório de exercícios no GitHub

Cada aluno deve ter **um repositório próprio**, na **sua própria conta do GitHub**, para armazenar os exercícios práticos das disciplinas.

> ⚠️ **É um único repositório para as duas disciplinas.** Dentro dele você separa o conteúdo em uma pasta por disciplina. Não crie um repositório para cada matéria.

Leia este guia até o fim antes de começar.

---

## 1. Requisitos

| O que | Para que serve | Link |
|---|---|---|
| Conta no GitHub | Hospedar seu repositório | [Criar uma conta](https://docs.github.com/pt/get-started/start-your-journey/creating-an-account-on-github) |
| Git instalado no computador | Enviar seus arquivos para o GitHub | [Download do Git](https://git-scm.com/downloads) |
| (Opcional) GitHub Desktop | Alternativa visual ao terminal | [GitHub Desktop](https://desktop.github.com/) |

> 💡 Como estudante, você tem direito ao **GitHub Student Developer Pack** (gratuito): https://education.github.com/pack

---

## 2. Nome do repositório (obrigatório)

O repositório **deve** ser nomeado com o seu RA, no formato:

```
ra-123456
```

Regras:

- tudo em **minúsculas**;
- prefixo `ra-` seguido do seu RA, **sem espaços** e **sem pontos**;
- substitua `123456` pelo **seu** RA real.

**Exemplos:**

| RA do aluno | Nome do repositório |
|---|---|
| 123456 | `ra-123456` |
| 987654 | `ra-987654` |

❌ Errado: `RA123456`, `ra 123456`, `Ra-123456`, `exercicios`, `trabalho-faculdade`, `ra-123456-programacao-web`

---

## 3. Estrutura de pastas (obrigatória)

Dentro do repositório, você terá **uma pasta por disciplina** e, dentro de cada uma, as pastas `aulas/` e `trabalhos/`:

```
ra-123456/
├── README.md
├── aaw/                     <- Arquitetura de Aplicações Web
│   ├── aulas/               <- exercícios práticos feitos em cada aula
│   │   ├── aula-01/
│   │   ├── aula-02/
│   │   └── ...
│   └── trabalhos/           <- trabalhos e entregas avaliativas
│       ├── trabalho-01/
│       └── ...
└── pw/                      <- Programação Web
    ├── aulas/
    │   ├── aula-01/
    │   ├── aula-02/
    │   └── ...
    └── trabalhos/
        ├── trabalho-01/
        └── ...
```

Pastas das disciplinas — use exatamente estes nomes:

| Pasta | Disciplina |
|---|---|
| `aaw/` | Arquitetura de Aplicações Web |
| `pw/` | Programação Web |

Regras de nomeação interna:

- **`aulas/`** — um subdiretório por aula: `aula-01`, `aula-02`, `aula-03`... (sempre com **dois dígitos**).
- **`trabalhos/`** — um subdiretório por trabalho: `trabalho-01`, `trabalho-02`...

> Se você cursa apenas uma das disciplinas, crie somente a pasta correspondente.

---

## 4. Passo a passo — criando o repositório no site do GitHub

1. Faça login em https://github.com
2. Clique no **`+`** no canto superior direito → **New repository**
   (ou acesse direto: https://github.com/new)
3. Preencha:
   - **Repository name:** `ra-123456` (com o **seu** RA)
   - **Description:** `Exercícios práticos - Arquitetura de Aplicações Web e Programação Web`
   - **Visibilidade:** `Public` (ou `Private` — veja o item 7)
   - ✅ Marque **Add a README file**
4. Clique em **Create repository**

📖 Documentação oficial: [Criar um novo repositório](https://docs.github.com/pt/repositories/creating-and-managing-repositories/creating-a-new-repository)

---

## 5. Criando as pastas

O Git **não versiona pastas vazias**. Por isso, cada pasta precisa conter pelo menos um arquivo.

### Opção A — pelo site do GitHub (mais simples)

1. No seu repositório, clique em **Add file** → **Create new file**
2. No campo do nome do arquivo, digite:
   ```
   aaw/aulas/README.md
   ```
   Ao digitar cada `/`, o GitHub cria a pasta automaticamente.
3. Escreva algo no arquivo, por exemplo: `# AAW - Aulas`
4. Clique em **Commit changes**
5. Repita o processo para os outros três caminhos:
   ```
   aaw/trabalhos/README.md
   pw/aulas/README.md
   pw/trabalhos/README.md
   ```

📖 Documentação: [Adicionar um arquivo a um repositório](https://docs.github.com/pt/repositories/working-with-files/managing-files/adding-a-file-to-a-repository)

### Opção B — pelo terminal (recomendado para praticar Git)

```bash
# 1. Clone o repositório (troque SEU-USUARIO e o RA)
git clone https://github.com/SEU-USUARIO/ra-123456.git
cd ra-123456

# 2. Crie as pastas com um arquivo dentro
mkdir -p aaw/aulas aaw/trabalhos pw/aulas pw/trabalhos
echo "# AAW - Aulas" > aaw/aulas/README.md
echo "# AAW - Trabalhos" > aaw/trabalhos/README.md
echo "# PW - Aulas" > pw/aulas/README.md
echo "# PW - Trabalhos" > pw/trabalhos/README.md

# 3. Envie para o GitHub
git add .
git commit -m "Cria estrutura de pastas das disciplinas"
git push origin main
```

> No **PowerShell** (Windows), troque o `mkdir -p ...` por:
> `mkdir aaw\aulas, aaw\trabalhos, pw\aulas, pw\trabalhos`

📖 Documentação: [Clonar um repositório](https://docs.github.com/pt/repositories/creating-and-managing-repositories/cloning-a-repository)

---

## 6. Rotina de trabalho em cada aula

```bash
# Antes de começar: puxe as atualizações
git pull

# Crie a pasta da aula do dia, dentro da disciplina certa
mkdir pw/aulas/aula-01

# ... faça os exercícios dentro dessa pasta ...

# Ao final da aula, envie seu trabalho
git add .
git commit -m "PW - Aula 01 - exercícios de <tema>"
git push
```

Use um prefixo na mensagem de commit para identificar a disciplina: `AAW - ...` ou `PW - ...`.

Comandos essenciais:

| Comando | O que faz | Documentação |
|---|---|---|
| `git status` | Mostra o que mudou | [Sobre o Git](https://docs.github.com/pt/get-started/using-git/about-git) |
| `git add .` | Prepara os arquivos para o commit | [Git cheat sheet](https://education.github.com/git-cheat-sheet-education.pdf) |
| `git commit -m "msg"` | Salva um ponto na história do projeto | [Sobre commits](https://docs.github.com/pt/pull-requests/committing-changes-to-your-project/creating-and-editing-commits/about-commits) |
| `git push` | Envia os commits para o GitHub | [Push commits](https://docs.github.com/pt/get-started/using-git/pushing-commits-to-a-remote-repository) |
| `git pull` | Traz as atualizações do GitHub | [Getting changes](https://docs.github.com/pt/get-started/using-git/getting-changes-from-a-remote-repository) |

> ⚠️ **Faça `commit` e `push` ao final de toda aula.** O que não estiver no GitHub não será considerado entregue. Veja os prazos na **seção 7**.

---

## 7. Acompanhamento de participação e prazos

As atividades de fixação e aulas práticas são acompanhadas pelo histórico de commits do repositório. Consulte o documento completo com regras, prazos e tabela de datas:

📄 **[PARTICIPACAO-AULAS-2026.2.md](PARTICIPACAO-AULAS-2026.2.md)**

> ⚠️ **Resumo:** cada atividade deve ser commitada em até **2 dias corridos** após a aula. Exceção: aulas 01–04 podem ser entregues até **10/09/2026**.

---

## 8. Repositório público ou privado?

- **Público:** qualquer pessoa vê seu código. Ótimo para seu portfólio.
- **Privado:** só você vê. Neste caso, **você precisa dar acesso ao professor** como colaborador.

📖 Documentação:
- [Definir visibilidade do repositório](https://docs.github.com/pt/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/setting-repository-visibility)
- [Convidar colaboradores](https://docs.github.com/pt/account-and-profile/setting-up-and-managing-your-personal-account-on-github/managing-access-to-your-personal-repositories/inviting-collaborators-to-a-personal-repository)

---

## 9. Modelo de README.md do seu repositório

Copie e adapte no arquivo `README.md` da raiz:

```markdown
# RA 123456 — Seu Nome Completo

Repositório de exercícios práticos das disciplinas cursadas em **2026.2**.

- Curso: <seu curso>
- Professor: Thalles Noce

## Disciplinas

| Pasta | Disciplina |
|---|---|
| `aaw/` | Arquitetura de Aplicações Web |
| `pw/`  | Programação Web |

Em cada disciplina:

- `aulas/` — exercícios práticos realizados em aula
- `trabalhos/` — trabalhos e entregas avaliativas
```

📖 [Sobre o README](https://docs.github.com/pt/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-readmes)
📖 [Sintaxe Markdown](https://docs.github.com/pt/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)

---

## 10. Checklist de entrega

Antes de enviar o link ao professor, confirme:

- [ ] Repositório criado na **minha** conta do GitHub
- [ ] Nome no formato `ra-<meu RA>` (minúsculas, sem sufixo de disciplina)
- [ ] Pasta da disciplina criada (`aaw/` e/ou `pw/`)
- [ ] Dentro de cada disciplina existem `aulas/` e `trabalhos/`
- [ ] `README.md` na raiz com meu nome e RA
- [ ] Link do repositório enviado ao professor:
      `https://github.com/SEU-USUARIO/ra-123456`

---

## 11. Problemas comuns

| Problema | Solução |
|---|---|
| Pediu senha no `git push` e recusou | O GitHub não aceita mais senha. Use um **Personal Access Token**: [criar token](https://docs.github.com/pt/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens) |
| `fatal: not a git repository` | Você não está dentro da pasta do projeto. Use `cd ra-123456` |
| A pasta vazia não aparece no GitHub | O Git ignora pastas vazias — crie um `README.md` dentro dela |
| Errei o nome do repositório | Dá para renomear: [renomear repositório](https://docs.github.com/pt/repositories/creating-and-managing-repositories/renaming-a-repository) |
| Já criei um repositório separado por disciplina | Renomeie um deles para `ra-<seu RA>` e mova os arquivos para dentro da pasta da disciplina |
| Quero desfazer algo no Git | [Git cheat sheet](https://education.github.com/git-cheat-sheet-education.pdf) e [Pro Git](https://git-scm.com/book/pt-br/v2) |

---

## Documentação oficial recomendada

- [GitHub Docs (português)](https://docs.github.com/pt)
- [Introdução ao GitHub — Guia rápido](https://docs.github.com/pt/get-started/start-your-journey/hello-world)
- [Sobre o Git](https://docs.github.com/pt/get-started/using-git/about-git)
- [GitHub Skills — cursos práticos gratuitos](https://skills.github.com/)
- [Pro Git — livro completo em português](https://git-scm.com/book/pt-br/v2)
