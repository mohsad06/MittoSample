# Mitto Sample

An example service application to send SMS and manage the data related to it. It's implemented using .NET Core 3.1 and the latest version of ServiceStack and ServiceStack.OrmLite which connects to a MySQL database.

## Project Structure

By using ServiceStack web template, multiple projects got created. All possible efforts have been made so that the implementation and separation of concepts follow the guidelines mentioned in ServiceStack documentation. Below, each project is explained.

### MittoSample

This project is the glue that keeps all other projects together. It's the main project that runs when the application starts and it's a .NET Core 3.1 web application. The most important part of this project is the `Startup` class which contains the introduction of `AppHost` class. `AppHost` inherits from `ServiceStack.AppHostBase` class and it's responsible for settings up the database and registering dependencies. The dependencies here (repository and logic) are being used in other projects and will be explained later.

### MittoSample.Model

Not included in the ServiceStack default template. This .NET Standard class library has consisted of simple POCO classes related to the two main entities: `SMS` and `Country`. These classes are responsible for shaping the database schema and store data. There's another POCO class here, `SMSGroupBy`, which is introduced to shape the data coming from the database that doesn't belong to the other entities. The repository and logic layers will work with these classes.

### MittoSample.Logic

Not included in the ServiceStack default template. This .NET Standard class library has consisted of two main layers. First, the repository layer, that is responsible for querying the database and providing raw data to the logic layer. The logic layer is kind of a wrapper for the repository layer. Logic layer not only hides and encapsulates the repository layer but also shapes the data where necessary and applies business logic (e.g. the logic for actually sending SMS to the receiver) and provides results to the ServiceInterface layer. The usage of the repository layer in logic and also the usage of the logic layer in the service interface has been implemented using IOC. These dependencies have been registered in the `Startup` class. While there are quite a lot of discussions and articles about advantages and disadvantages of Repository Pattern, Generic Repository and UnitofWork pattern, I've tried not to add unnecessary abstraction layers and also committed to keeping my repository pattern simple, effective, and based on the domain and requirements of the project.

### MittoSample.ServiceModel

This project, as mentioned in ServiceStack documentation, contains the necessary request and response DTOs. The routing used inside DTO classes is designed to handle the services mentioned in the Task Definition document. Since ServiceStack has built-in support to return `XML` and `JSON` responses, there's no need to specify the request format in the route.

### MittoSample.ServiceInterface

Finally, the last project is responsible for handling requests coming toward the application based on the shape of the request. Since all requested services needed to handle `HTTP GET` verb, all methods inside the classes of this project are called Get, but with different signatures. The logic layer dependency is injected here in order to keep this project clean and let it focus on shaping proper responses.

## Difficulties and Missing Parts

Unfortunately, since I did not want to exceed the time limit for implementing this task and some complications of ServiceStack in terms of writing unit and integration tests, this part of the project is incomplete. Also, the process of deploying the application to Docker, due to lack of documentation and usage of ServiceStack, didn't go as planned and proved to be more complex than first thought. However, given more time I'm confident that I'll be able to finish these two parts as well.

## Positive Notes

ServiceStack is truly a brilliant framework. Its emphasis on simplicity and transformation of the way web services should work is just a couple of its strengths. It's clear that a lot of time and energy has been put into this framework because almost everything is considered very carefully. For example, having an ORM framework (OrmLite) or a container that handles IOC.

## Optimisations

One of the things that could improve the practicality of this application is implementing authentication and authorization (which again, ServiceStack already has plugins for it) and a lightweight user management system. Also, the behaviour of the application in terms of the incorrect format of incoming requests is not clearly explained. For example, if date is not in the correct format what should happen? Although, by using ServiceStack ResponseStatus all exceptions could be managed when client consumes services. The application is using a top-to-bottom asynchronous structure to avoid processes blocking each other, but in terms of performance, profiling could be used to find the bottlenecks and improve them. In terms of data management, other ORMs could be considered after inspecting their performance reports.
