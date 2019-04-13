# Event Management Middleware
A management middleware that lets you manage all your event from one place. (under construction)

### Commonly used event management system
![OldEventManagement](https://user-images.githubusercontent.com/48536631/54880706-db02e780-4e58-11e9-876e-641ac55c98ed.png)

As you can see here, n number of services are running. And they have n number of sub service to subscribe broker.  
Suppose that the message posted in a service has changed. If you are using some system like this, you have to change every piece of code in every service which subscribe for this message.  
For example if you have adopted the microservices architecture, after a while this progress will be a very laborious task for you.

### Our New Event Management Middleware
![NewEventManagement](https://user-images.githubusercontent.com/48536631/54880672-44cec180-4e58-11e9-99d9-0d8ad4a61722.png)
Suppose that the same system is developed using event management middlware and the same message has changed.  
All you need to do is go to management system and change a few line code logic.  
All Done.  
No need to republish any other services.  
No need to do anything else.
## Example
 https://github.com/demirmusa/FlowBasedEventManagementMiddlewareExample
