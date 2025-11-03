🔧 Garantia de Aparelhos – Backend

API backend de solicitação de garantia de aparelhos eletrônicos, com processos automatizados via Camunda BPMN e regras de decisão DMN.

🎯 Objetivo

Automatizar o fluxo de garantia: do envio da solicitação pelo cliente até a análise técnica e decisão final (reparo ou troca), garantindo persistência e integridade dos dados.

🛠 Tecnologias

.NET Web API (C#) – endpoints REST

Camunda BPMN 2.0 – processos de negócio

DMN – regras de decisão (elegibilidade, análise técnica)

PostgreSQL – banco de dados

JSON – troca de dados com frontend

📂 Estrutura do Projeto
/backend
  /bpmn              # Processos BPMN
  /dmn               # Regras de decisão
  /controllers       # Endpoints da API
  /models            # Models (Usuário, Aparelho, Solicitação)
  /services          # Lógica de negócio e integração com Camunda
  Program.cs         # Configuração da aplicação
  appsettings.json   # Configurações do banco e Camunda

🔄 Fluxo do Backend

📝 Cliente envia solicitação via API

✅ Backend valida dados do usuário e aparelho

🔄 Solicitação é enviada para Camunda (BPMN)

⚖️ Regras DMN avaliam elegibilidade e defeito

📊 Status final persistido no banco e retornado ao cliente

🔍 Usuário pode consultar ou deletar solicitações

📝 Endpoints
Método	Endpoint	Descrição
POST	/api/solicitacao	Envia nova solicitação
GET	/api/solicitacao	Lista solicitações do usuário
GET	/api/solicitacao/{id}	Detalhes de uma solicitação
DELETE	/api/solicitacao/{id}	Remove solicitação finalizada
✅ Regras Implementadas

Validação de dados do cliente (nome, CPF, celular)

Validação de dados do aparelho (marca, modelo, nota fiscal)

Verificação automática da elegibilidade da garantia (DMN)

Análise técnica automatizada conforme tipo de defeito

Escolha entre reparo ou troca baseada em critérios técnicos
