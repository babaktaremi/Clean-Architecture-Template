
[![NuGet Badge](https://buildstats.info/nuget/Bobby.CleanArcTemplate)](https://www.nuget.org/packages/Bobby.CleanArcTemplate)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://makeapullrequest.com)
![workflow](https://github.com/babaktaremi/Clean-Architecture-Template/actions/workflows/package.yml/badge.svg)


 <a href="https://www.linkedin.com/in/babak-taremi/" alt="Connect on LinkedIn">
   <img src="https://img.shields.io/badge/-Babak%20Taremi-0072b1?style=flat&logo=Linkedin&logoColor=white" />
 </a>
 
  <a href="mailto:babaktaremi@yahoo.com" alt="Email">
  <img src="https://img.shields.io/badge/-Babak%20Taremi-0072b1?style=flat&logo=yahoo&logoColor=white&color=purple" />
 </a>
 
  <a href="https://t.me/BoB_Tm" alt="My Telegram">
   <img src="https://img.shields.io/badge/-Babak%20Taremi-0072b1?style=flat&logo=telegram&logoColor=white&color=darkblue" />
 </a>

# ASP NET Core 8 Clean architecture boiler plate. Everything that you need to start an enterprise project!

Personally I've seen a lot of ASP NET Core based clean architecture templates, but most of them are not usable in production and you face problems created by the framework itself rather than the actual domain problems. I've worked for about 5 years in different startups and companies so trust me when I say this framework was built upon actual real-world challenges rather than reading a book and convert its every paragraph to code.

So let's get started!

# Good to know

Check [This link](https://learn.microsoft.com/en-us/visualstudio/ide/file-nesting-solution-explorer?view=vs-2022) for better and cleaner project solution view in Visual Studio!


# Quick and easy installation

Open powershell or command prompt and execute the following command:

 ```
dotnet new install Bobby.CleanArcTemplate
 ```
 
 Create an empty folder. Navigate to it via Powershell or CMD and then execute the following command:
 
  ```
dotnet new ca-template
 ```
Your project is created and ready to code! 


# Using Docker File and docker compose

First you need to generate a self-signed SSL certificate. Open Terminal and run the following command:

```
 dotnet dev-certs https -ep $env:USERPROFILE/.aspnet/https/cleanarc.pfx -p Strong@Password
```

Now for trusting the certificate , run the following command:

```
dotnet dev-certs https --trust
```

Navigate to the project directory and run the following command using your prefered terminal:

```
 docker build -t bobby-cleanarc -f dockerfile.
```

and lastly run the following command ( Note that this command will pull the latest SQL Server 2022 image from docker hub):

```
docker-compose up -d
```


## Clean Architecture. Our Expectations
When you search the **"Clean Architecture"** keyword on google, the first thing that comes in search results is the graph below. But what does it say exactly?

![](https://i.ibb.co/vxpLVCp/Clean-Architecture-Diagram2.webp)

### Domain Layer 
 Our **core** (or **"Domain"**) becomes completely **isolated** and changes in other layers won't affect our core services and logics.

### Application Layer
The second most important layer is the **"Application"** layer (**together with "Domain" layer ,they construct the backbone of our entire project**). If you think of the project as a human body, the "Domain" is the brain of this human and the "Application" is the body of this human. So "Application Layer" acts somewhat as a "Request Orchestrator" and routes each request to its proper Domain model.
"Application Layer" also define a set of "Contracts" in which the project acts upon them. **Application and Domain layer don't care about the implementation details of these contracts, All they know is the signatures of these contracts and they act upon these signatures.**
so in this way our project becomes testable and we can begin our development without the need to worry about anything else (such as database provider, Identity, logging, messaging etc...)
. you can see that the core of our project has become completely agnostic and self managed.
### Infrastructure Layer

This layer provides the things that our project needs in order to work in **real world**. "Infrastructure" layer often **implements the contracts defined in "Application" layer**. This is the layer that determines the database that we want to use, messaging service implementation ,user authentication mechanism etc...

With this approach we can easily change the services that we want to. All we need to do is to **swap the implementation of desired contract defined in "Application" layer** to the one that we want.

### WebUI Layer

In this layer, we decide about **how do we want to present our project**. It can be ASP NET Core web API, WPF, GRPC Server, Blazor Server etc...
this layer becomes our running instance of the project.


##  Dive Into The Logic!

Let's take a look at each layer and review its purpose.

### SharedKernel

This layer (or Class Library) is where we define our extension methods or other common userful methods which we may use in any other layer. So each layer must have a reference to it. 

### Domain

The core or the **"Brain"** of our project. Each Domain Entity can have their own specific methods and behaviors. In order to explicitly define these models, we create a common abstract entity called **"BaseEntity"** and inherit our Domain entities from it. In this way we have marked our **"Domain Entities"** and later we can use reflection or source generators to automate a part of development process, resulting in writing less boring and repetitive code. (You can see the actual example in **"Infrastructure.Persistence"** where we use reflection to find DB Models to generate migrations) 

### Application

As said before, this layer is responsible for **request routing** and defining **contracts** (often as **"Interfaces"**) that our project needs. These contracts will be implemented in other levels.
Because this layer handles request routing, this layer is also the best candidate to implement **"CQRS Pattern"**, and this can be achieved with the popular package that we all know of. **THE MEDIATR** !

### Unit Tests

One of the core aspects of clean architecture is that each layer of solution should be testable. As you probably know , setting up test environment can be tiresome and repetitive task. this framework provides basic test setups needed to help you solely focus on writting unit tests. Also you are free to write unit tests for all the layers that you want



We have a set of **"Features"** in this layer where we mostly act upon the result of "Domain Services". In order to follow CQRS best practices , we separate **"Command"** and **"Query"** models. MediatR will take care of the rest of the work for us and calls their related handlers automatically.

### CleanArc.Infrastructure.CrossCutting

Cross cutting services are services that cross multiple layers and don't specifically related to a service or context. For example **logging service** is a service that all other services will use it.

### CleanArc.Infrastructure.Identity

Implementation of services for user registration , authentication and authorization. We use **ASP NET Core Identity** package since it has many features already implemented and many security considerations are already applied. Also **"Dynamic Access Control"** and **"JWE-Token Base authentication"** and **"OTP authentication"** services are already implemented in this layer. (Just check the codes. It's not that hard to get a handle on)

### CleanArc.Infrastructure.Persistence

We have seen many examples of **"Repository"** and **"Unit of Work"** pattern all around the net. In my opinion, most of them just add more complexity without any benefits. many even could argue that with EF Core , we don't need these old patterns. In some extent, I agree **BUT** I also think that many of "Repository" and "Unit of Work" implementations are done completely **wrong**. Having a Repository with **self-descripting** name and purpose help us to encapsulate the logic and technology behind **"Data Access Strategy"**. This gives us the benefit of being **"Persistence Agnostic"** which means that we don't rely on database provider system and we can swap our database completely if we want to.
Also having "Unit of Work" helps us to maintain **"Atomicity"** and **"Consistency"** in our database transactions and **execute multiple transactions in one request** which **changes the database state from one valid state to another**.

### CleanArc.WebFramework

As the project progresses, the number of different services and configurations increase. Soon you will face a **polluted Program class with thousand lines of duplicated codes just for service configurations.**
This layer help us to **separate each configuration to its own class**. Thus the maintenance and debugging becomes much easier and you can **write configurations that are reusable**. 

### CleanArc.Web.Api

**"The Presentation"** of the whole project. I think **"ASP NET Core Web API"** is the most appropriate approach since it covers most things that we need from a Presentation Layer.

### CleanArc.Web.Plugins

**NOW THERE IS THE WINNING POINT OF THIS FRAMEWORK!**

You might wonder "Wait a minute ! We can't have Plugins in a Web API framework!" But we can!

Plugins are the sweet spot between **monolithic applications** and **microservices architecture** . With plugins your code become **modular** while maintaining **"High Cohesion"** without **common microservice development disadvantages** (like high costs of developing and maintaining microservice applications). 

You might have heard something called **"Application Parts"** in ASP NET Core. In general, **Application Parts are the place that controller classes bocome Http endpoints, C# classes become json values, service lifetimes and implementations are configured etc.**
It is very important and sensitive part of whole ASP NET Core configuration. If you don't be careful enough, you can mess up the whole project in a heartbeat. 
But you don't need to worry about any of it. I've done the most work for you! (Take a look at **CleanArc.Web.Plugins.Grpc** class library). 

## Final Words

Personally I love this framework and use it in many of my personal projects and it hasn't let me down yet. I casually update this repo (whether it's a package update, code refactor or adding new features). So feel free to create issues or create pull requests. I will check them.

If you like this framework , just give it a star. Your star keeps me motivated to maintain this repo and develop new open source and exciting packages for it. Thanks in advance!

## Code Inspirations

[ MJ Ebrahimi Complete Web API Repo](https://github.com/mjebrahimi/AspNetCore-WebApi-Course)

[ Clean Arc By Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)
