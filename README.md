<p align="center">
    <img width="96" src="./icon.png" />
</p>

# Narwhal

This repository contains a miniature version of the infrastructure we run in OneOcean. We use this repo as a test during our hiring process.

It is a full web application, displaying a world map, and showing points where [Navigational Warning](https://www.ccg-gcc.gc.ca/mcts-sctm/navwarn-avnav-ca-eng.html) from [France](https://gan.shom.fr/diffusion/home) are emitted.

<p align="center">
    <img width="560" src="./screenshot.png" />
</p>

The goal is to provide a test canvas for any candidate to ask various tasks and questions while staying close enough to our real world needs.
It contains 8 different components, using multiple applications, tools and languages.

This test framework is valid for any technical position, as it includes some infrastructure, web services, front-end application, ETL pipelines and more.


## Structure

- **Narwhal.App**: Web application, built with Angular. It sits behind the reverse proxy and communicates with the service to get information. It displays a world map and some
- **Narwhal.Database**: A MongoDB storage used to store information collected through the ETL pipeline
- **Narwhall.ETL**: A simple ETL pipeline written in Python, reads a text file to extract information and stores it in the database
- **Narwhal.Front**: A reverse proxy sitting in fromt of all web endpoints, similar to a real infrastructure. Any application or service call will go through it. It uses Nginx
- **Narwhal.Importer**: A Powershell script used to import the text based file for processing using the ETL pipeline
- **Narwhal.Messaging**: A MQTT server (mosquitto) used to notify the service, and the application when new data is available
- **Narwhal.Service**: An ASP.NET Core web service written in C#. It exposes the database information and reacts when new data is available
- **Narwhal.Tests**: A test suite for this application, writte in Python

Two scripts are available inside of every service directory:
- **start_local.bat**: A batch used to quickly start the corresponding service locally on your machine
- **start_docker.bat**: A batch used to build a Docker image and run it on your machine

Scripts with the same name are available in the root directory to run the full service stack at once. Please note that the root **start_docker.bat** script will use Docker Compose.

<p align="center">
    <img width="560" src="./Narwhal.drawio.png" />
</p>


## Requirements

You will be able to run all the services only using Docker on your machine.
As mentionned, we also provide a **docker-compose.yml** file.

To develop on this application locally, you will need:
- **Powershell** to run **Narwhal.Importer**
- **Python** to run **Narwhal.ETL** and **Narwhal.Tests**
- **.NET Core 3.1** to run **Narwhal.Service**
- **NodeJS** to run **Narwhal.App**

Even though the scripts are only provided in .bat files, the services should not be platform dependent and should be usable on Linux or Mac.


## Attribution

- Icon by [Freepik](https://www.flaticon.com/authors/freepik) from [www.flaticon.com](https://www.flaticon.com/)
- MongoDB downloaded from [www.mongodb.com](https://www.mongodb.com/try/download/community)
- Nginx downloaded from [nginx.org](http://nginx.org/en/download.html)
- Mosquitto downloaded from [mosquitto.org](https://mosquitto.org/download/)
- Fart downloaded from [sourceforge.net/projects/fart-it](https://sourceforge.net/projects/fart-it/)