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
  "rabbitmq-address": "localhost"
}
```

## 3. Configure My SQL Environment

## 3.1 Run My SQL in a docker container
```
docker run
```

### 3.2 Create the Database and Table for saving Messages

Use the script: [#script.sql](/assets/script.sql)

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
