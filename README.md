### Chat Web Solution


###Run RabbitMQ in docker environment

``docker run -d -p15672:15672 -p5672:5672 --hostname my-rabbit --name rabbit rabbitmq:3-management``


###Configure secrets.json

``
{
  "StooqUrl": "https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv",
  "rabbitmq-address": "localhost"
}
``