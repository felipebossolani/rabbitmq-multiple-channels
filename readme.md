# .NET Core + RabbitMQ - Multiplos Canais

Olá esse é um projeto de estudos e testes em .net core para entender o mecanismo de multiplos canais para uma mesma fila no RabbitMQ.
A instância do RabbitMQ está contida no arquivo de DockerCompose e para executa-lo basta roodar o comando abaixo na raiz do projeto:
```
docker-compose up -d
```

# Projetos

Temos dois projetos: 
### Publisher
Que é o responsável por publicar as mensagens. Basta dar um run nesse projeto para iniciar o envio de mensagens.

### Consumer
Que é o responsável por ler a fila e consumir as mensagens.

### Referências
Playlist do Brandão no Youtube: https://www.youtube.com/playlist?list=PLfhPyyHRfeug87iBjkAP2ulwcqObiO_fW
Série do Gago.io: https://gago.io/categoria/trilhas/rabbitmq-a-z/
