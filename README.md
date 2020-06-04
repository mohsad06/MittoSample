# Mitto Sample

An example service application to send SMS and manage the data related to it. It's implemented using .NET Core 3.1 and the latest version of ServiceStack and ServiceStack.OrmLite which connects to a MySQL database.

## Task Definition

Implement some web services for sending SMS, viewing sent SMS and aggregated statistics. The data shall be stored in a relational database. During send an SMS the mobile country code of the receiver needs to be identified and stored within the SMS record (list of countries and the codes could be found below). The logic for sending the SMS will be implemented later, so it needs to send the message to a dummy implementation, which just dumps the SMS to a log file. A later exchange of the dummy implementation with the real logic should not require changes on the written code.
All the services should be able to return the responses in JSON and XML format. The services need to fit the interface definition below.

### List of Countries
 - Name: Germany, MobileCountryCode: 262, CountryCode: 49, PricePerSMS: 0.055
 - Name: Austria, MobileCountryCode: 232, CountryCode: 43, PricePerSMS: 0.053
 - Name: Poland, MobileCountryCode: 260, CountryCode: 48, PricePerSMS: 0.032
 
### Service Interface Definition

#### SendSMS

Parameters: from: string [the sender of message], to: string [the receiver of message], text: string [the text which should be sent]

Returns: state: enum (success, failed)

URL Format:
- GET /sms/send.json?from=The+Sender&to=%2B4917421293388&text=Hello+World
- GET /sms/send.xml?from=The+Sender&to=%2B4917421293388&text=Hello+World

#### GetCountries

Parameters: None

Returns: An array of countries containing: mcc: string [the mobile country code of the country e.g. "262" for Germany], cc: string [the country code of the country e.g. "49" for Germany], name: string [the name of the country e.g. "Germany"], pricePerSMS: decimal [the price per SMS sent in this country in EUR, e.g. 0.06]

URL Format:
- GET /countries.json
- GET /countries.xml

#### GetSentSMS

Parameters: dateTimeFrom: dateTime [format: “yyyy-MM-ddTHH:mm:ss”, UTC], dateTimeTo: dateTime [format: “yyyy-MM-ddTHH:mm:ss”, UTC], skip: integer [skip n records], take: integer [take n records]

Returns: totalCount: int [the total count of all items matching the filter], items: array of SMS containing: dateTime: dateTime [the date and time the SMS was sent, format: “yyyy-MMddTHH:mm:ss, UTC], mcc: string [the mobile country code of the country where the receiver of the SMS belongs to], from: string [the sender of the SMS], to: string [the receiver of the SMS], price: decimal [the price of the SMS in EUR, e.g. 0.06], state: enum (Success, Failed)

URL Format:
- GET /sms/sent.json?dateTimeFrom=2018-03-01T11:30:20&dateTimeTo=20180302T09:20:22&skip=100&take=50
- GET /sms/sent.xml?dateTimeFrom=2018-03-01T11:30:20&dateTimeTo=20180302T09:20:22&skip=100&take=50

#### GetStatistics

Parameters: dateFrom: date [format: “yyyy-MM-dd”], dateTo: date [format: “yyyy-MM-dd”], mccList: string list [a list of mobile country codes to filter, e.g. “262,232”] if list is empty this means include all mobile country codes.

Returns: Array of statistic records containing: day: date [format: “yyyy-MM-dd”], mcc: string [the mobile country code], pricePerSMS: decimal [the price per SMS in EUR, e.g. 0.06], count: integer [the count of SMS on the day and mcc], totalPrice: decimal [the total price of all SMS on the day and mcc in EUR, e.g. 23.64]

URL Format:
- GET /statistics.json?dateFrom=2018-03-01&dateTo=2018-03-05&mccList=262,232
- GET /statistics.xml?dateFrom=2018-03-01&dateTo=2018-03-05&mccList=262,232



## Project Structure

By using ServiceStack web template, multiple projects got created. All possible efforts have been made so that the implementation and separation of concepts follow the guidelines mentioned in ServiceStack documentation. Below, each project is explained.

### MittoSample

This project is the glue that keeps all other projects together. It's the main project that runs when the application starts and it's a .NET Core 3.1 web application. The most important part of this project is the `Startup` class which contains the introduction of `AppHost` class. `AppHost` inherits from `ServiceStack.AppHostBase` class and it's responsible for settings up the database and registering dependencies. The dependencies here (repository) are being used in other projects and will be explained later.

### MittoSample.Model

Not included in the ServiceStack default template. This .NET Standard class library has consisted of simple POCO classes related to the two main entities: `SMS` and `Country`. These classes are responsible for shaping the database schema and store data. There's another POCO class here, `SMSGroupBy`, which is introduced to shape the data coming from the database that doesn't belong to the other entities. The repository layer will work with these classes.

### MittoSample.Repository

Not included in the ServiceStack default template. This .NET Standard class library is responsible for querying the database and providing data to the ServiceInterface layer. The usage of the repository layer in the service interface layer has been implemented using IOC. These dependencies have been registered in the `Startup` class. While there are quite a lot of discussions and articles about advantages and disadvantages of Repository Pattern, Generic Repository and UnitofWork pattern, I've tried not to add unnecessary abstraction layers and also committed to keeping my repository pattern simple, effective, and based on the domain and requirements of the project.

### MittoSample.ServiceModel

This project, as mentioned in ServiceStack documentation, contains the necessary request and response DTOs. The routing used inside DTO classes is designed to handle the services mentioned in the task definition section. Since ServiceStack has built-in support to return `XML` and `JSON` responses, there's no need to specify the request format in the route.

### MittoSample.ServiceInterface

Finally, the last project is responsible for handling requests coming toward the application based on the shape of the request and applying business logic (e.g. the logic for actually sending SMS to the receiver). Since all requested services needed to handle `HTTP GET` verb, all methods inside the classes of this project are called Get, but with different signatures. The repository layer dependency is injected here in order to keep this project clean and let it focus on shaping proper responses.

## Difficulties and Missing Parts

Unfortunately, since I did not want to exceed the time limit for implementing this task and some complications of ServiceStack in terms of writing unit and integration tests, this part of the project is incomplete.

## Positive Notes

ServiceStack is truly a brilliant framework. Its emphasis on simplicity and transformation of the way web services should work is just a couple of its strengths. It's clear that a lot of time and energy has been put into this framework because almost everything is considered very carefully. For example, having an ORM framework (OrmLite) or a container that handles IOC.

## Optimisations

One of the things that could improve the practicality of this application is implementing authentication and authorization (which again, ServiceStack already has plugins for it) and a lightweight user management system. Also, the behaviour of the application in terms of the incorrect format of incoming requests is not clearly explained. For example, if date is not in the correct format what should happen? Although, by using ServiceStack ResponseStatus all exceptions could be managed when client consumes services. The application is using a top-to-bottom asynchronous structure to avoid processes blocking each other, but in terms of performance, profiling could be used to find the bottlenecks and improve them. In terms of data management, other ORMs could be considered after inspecting their performance reports.
