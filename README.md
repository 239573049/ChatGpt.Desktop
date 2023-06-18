# ChatGpt.Desktop

English| [简体中文](./README.zh-CN.md)

## Introduction
ChatGpt.Desktop is a simple and easy-to-understand interface implemented using Blazor, which supports multiple conversations. It is available on Android, iOS, Mac, Linux, Windows, and Web platforms.

## Software Architecture
Blazor is used as a cross-platform UI, and Masa Blazor interface is used.

## Instructions
1. Click the settings button in the upper right corner.
2. Set the token. If you have a proxy server, you can modify the API address to your own proxy server.
3. Save the settings.
4. Send a message and get an answer.
5. Messages will be saved in the browser cache and can be cleared in the settings for the current conversation.

## Setting up ChatGpt Proxy
To set up a ChatGpt proxy, you need to prepare a server overseas, such as in Singapore or another country. You also need to have Docker and Docker Compose installed. Use the following script to deploy the proxy service. Note that the proxy service only proxies the api.openai.com interface. After deployment, set the `ApiUrl` in the application to the server's address, http://server_ip:server_port//v1/chat/completions.

```yml
services:
  chatgpt:
    image: registry.cn-shenzhen.aliyuncs.com/tokengo/chatgpt-gateway
    container_name: chatgpt
    ports:
      - 1080:80
```

## How to use Web Server

In the current project root directory there is a file 'docker-compose.yml', which can be run directly on the Server, it is a project image of Blazor Server, if it is deployed to a foreign server, there is no need to climb over the wall to access the ChatGpt Api

```yaml
services:
  chat-server:
    image: registry.cn-shenzhen.aliyuncs.com/tokengo/chat-server
    build:
      context: .
      dockerfile: ./src/ChatGpt.Server/Dockerfile
    container_name: chat-server
    ports:
      - 1800:80
```

## Contribute
1. Fork this repository
2. Create a new feature/xxx branch
3. Submit your code
4. Create a new Pull Request

Thanks to the following contributors:

<a href="https://github.com/239573049/ChatGpt.Desktop/graphs/contributors">   <img src="https://contrib.rocks/image?repo=239573049/ChatGpt.Desktop" /> </a>

## Preview
![img](./img/setting.png)
![img](./img/home.png)
![img](./img/home1.png)

## Get ChatGpt Token
To use this application, you need a ChatGpt account and login to create a token. Visit the following link to create a token: https://platform.openai.com/account/api-keys

## Conclusion
Welcome to contribute to this project. From Token with love. Join our QQ group for learning and communication: 737776595.

