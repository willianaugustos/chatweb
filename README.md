# Chat Web Solution
## William Augusto

The purpose of this project is to create a simple browser-based chat application using .NET



# To Configure environment, follow this steps:

## 1. Run RabbitMQ in docker environment

```
docker run -d -p15672:15672 -p5672:5672 --hostname my-rabbit --name rabbit rabbitmq:3-management
```


## 2. Configure secrets.json like this:

```
{
  "StooqUrl": "https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv",
  "rabbitmq-address": "localhost",
  "db-connectionstring": "server=localhost;database=chat;user=root;password=1234"
}
```

Please note the tag <span style="color:orange;">{code}</span> in the StooqUrl configuration.


## 3. Configure My SQL Environment

## 3.1 Run My SQL in a docker container
```
docker run --name some-mysql -p3306:3306 -e MYSQL_ROOT_PASSWORD=1234 -d mysql:latest
```

### 3.2 Create the Database and Table for saving Messages

Use the script: [#script.sql](/assets/script.sql)\
You can do it by using Mysql Workbench or other similar tool.

In case you prefer do it by using by command lines, you have to use the following commands:

### Inside container, open mysql command line
```
docker ps

docker exec -it [container-id] bash

mysql -u root -p
```
Now you can run SQL Commands directly on the container.


### 4. Run the application using visual studio or dotnet cli

Using the Terminal Window, Go to folder ./chatweb/chatsolution/chatsolution, and run the following command:
```
dotnet run
```


# How it Works

- Use SignalR to manage websockets and keep conversation between browser and server\
![Chatting Window](/assets/Img1_Chatting.png)

- Identfy commands by searching "/[command]=" pattern
- If The command is "/stock={code}" then start a bot to work on background\
![Bot](/assets/Img2_BotWorking.png)

- Have MySQL to save messages, except Bot Messages\
![Database](/assets/Img3_DontSaveBotMessages.png)

- When a command is invalid, the background feedbacks immediately\
![Wrong Command](/assets//Img4_WrongCommandFeedback.png)

- The service behing the Bot is "stooq". This service returns information as following
- I used Regular Expression to extract the piece of information that is wanted:\
![Stooq](/assets/img_regex_stooq.png)
