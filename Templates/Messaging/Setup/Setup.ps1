#iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex

choco install erlang
choco install rabbitmq -y


cd "C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.5\sbin"

.\rabbitmq-service.bat start
.\rabbitmq-plugins enable rabbitmq_management

Start-Process "http://localhost:15672/#/queues"