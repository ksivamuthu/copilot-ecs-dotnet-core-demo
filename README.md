Howdy !! In the [previous post](https://ksivamuthu.hashnode.dev/aws-copilot-ecs) on [this series](https://hashnode.com/series/aws-copilot-ecs-ckhskugtp07r2s6s14k1l6s56), we had a walkthrough on how AWS Copilot facilitates creating the application infrastructure, container registry and build the container to deploy in the test environment. Yes, that's the Hello World .NET Core application - deployed in the test environment. 

Now, let's see how to set up the pipeline and automate the application release to test and production environments whenever the source code is committed. 

Let's start. We will add more to our demo code to build. That will also help us explore more in ECS like storage and service discovery in upcoming posts in this series. The "Hello World" .NET Core application is upgrading into "Coffee Service." Later we will add more services to our demo coffee shop application.


![rob-laughter-LWiXD-81bIg-unsplash.jpg](https://cdn.hashnode.com/res/hashnode/image/upload/v1606267559136/OnFTEjhdD.jpeg)

## ☕️ Coffee Shop Application Demo 

In this AWS Copilot series, we will develop the coffee shop application hosted in ECS Fargate using Copilot.

The microservices in this Coffee Shop Application demo is:

1. Coffee Order UI - Submitting Order
2. Coffee Service - API for Coffee types
3. Order Service - API for creating order and get the status of the order.
4. Barista Service - Processing order and update the status (Event Driven)


![CoffeeService.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606277832919/8mOAKqshn.png)

When we are deploying to ECS, we will go through what are the types of service and route path rules for each service.

1. Coffee Order UI - */* - It's the default route to navigate to the client application behind the load balancer.
2. Coffee Service - */coffee-service/** - The Coffee Service API is going to be prefixed with coffee-service behind the load balancer.
For, e.g., `https://awscopilotcoffeeshop.com/coffee-service/api/coffees` will be rewriting to the dot net core application `http://coffee-service.local:80/api/coffees`.
3. Order Service -  */order-service/** - The Order Service API will be prefixed with order-service behind the load balancer.
4. Barista Service - Internal Backend Service that is not exposed to the internet. The services/instances in the VPC can access them.

In this post, we will focus only on Coffee Service & Coffee Order UI development and setting up the pipeline to automatically deploy your container application.

### Coffee Service

Here comes my favorite part... Let's add the services that list the types of coffee; users can choose to order. The .NET core app with the endpoint is ready that returns the list of coffee types. The application is running in a container. Now, it's a single command to add it our application and deploy.

```
copilot svc init -n coffee-service
```

Select the location of the docker file to create the service. Once the service is created, take a look at the manifest file created under the copilot directory. In the HTTP section of the below manifest, you can see the path prefix `coffee-service` add. This will reflect in your application load balancer rules. 

You may need to add the coffee-service as PathBase in your .NET Core app to handle the requests properly from serving behind the ALB (Application Load Balancer)

```csharp
 app.UsePathBase(new PathString("/coffee-service"));
```

```
## File Path: copilot/coffee-service/manifest.yml

# Your service name will be used in naming your resources like log groups, ECS services, etc.
name: coffee-service
# The "architecture" of the service you're running.
type: Load Balanced Web Service

image:
  # Docker builds arguments.
  # For additional overrides: https://aws.github.io/copilot-cli/docs/manifest/lb-web-service/#image-build
  build: CoffeeService/Dockerfile
  # Port exposed through your container to route traffic to it.
  port: 80

http:
  # Requests to this path will be forwarded to your service. 
  # To match all requests, you can use the "/" path. 
  path: 'coffee-service.'
  # You can specify a custom health check path. The default is "/."
  # For additional configuration: https://aws.github.io/copilot-cli/docs/manifest/lb-web-service/#http-healthcheck
  healthcheck: '/healthz'
  # You can enable sticky sessions.
  # stickiness: true
```

You can deploy the service using the below command.

```
copilot svc deploy -n coffee-service --environment test
```

Test the service with the curl command

```bash
curl -s http://<loadbalancer_url>/coffee-service/api/coffee | jq
```

![Screen Shot 2020-11-24 at 11.47.46 PM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606279886100/UYgvoKmoj.png)

### Add more containers...  Coffee Ordering UI

The Coffee Ordering UI is created as a React application that fetches the coffee types from the API and listing them for the user to choose from. The React application is built and serves as static files from the Nginx container.  

Create a new Load Balancer Web Service to serve this UI app to the internet. 

```
copilot svc init -n coffee-ordering-ui
```

Select the location of the docker file to create the service. Once the service is created, verify and modify the manifest file as needed, and you can deploy the service using the below command.

```
copilot svc deploy -n coffee-ordering-ui --environment test
```

The app looks like this... 

![Screen Shot 2020-11-25 at 2.10.09 PM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606338020747/vBHlVV264.png)

## Setup the pipeline to automate the release

The key principle of the devops process is "Ship small, Ship often." The process of deploying small features on a regular cadence is crucial to DevOps. As the team is becoming more agile, we need to automate application releases as multiple developers, multiple services teams pushing the code into the source code repository. 

AWS Copilot tool can help you in setting up the pipeline to automate application releases. You can run these commands to create an automated pipeline for the application that builds and deploy the application on git push. You need to set up the github repo personal access token that has permissions includes reading the repo.

```bash
copilot pipeline init
copilot pipeline update // To update
git push
```

Check the status of the pipeline using copilot cli or in the AWS Code Pipeline console.

```bash
copilot pipeline status
```

![Screen Shot 2020-11-25 at 12.06.28 AM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606281524686/6OdnYgEZT.png)

![Screen Shot 2020-11-25 at 12.18.01 AM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606281490326/wtKFK9-jD.png)

Now, it's ready to bring more developers to start rocking. They don't need to install a copilot to deploy the machine's service, as they are developing. The deployment steps are automated.

## Pushing more code

I see the UI we've created needs more things to add. I'm going to add the image of the coffee in the app and service. Once I pushed the repository changes, the pipeline deploys the updated version of the app and service in the test or integration environment automatically.

The new version of the app deployed:

![Screen Shot 2020-11-25 at 3.37.48 PM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606337951506/83F-eMgPJ.png)

## Deploying to Production

In this application, we have a `test` environment in the pipeline. Let's add another environment `production`. We will set up the manual approval as the required action as the approval to the changes going into the gateway.

In the pipeline.yml created at the step of initializing the pipeline, add prod deploy as one stage with requires_approval as true. Update the pipeline using `copilot pipeline update` after these changes.

```yml
stages:
    - name: test
      # Optional: flag for manual approval action before deployment.
      # requires_approval: true
      # Optional: use test commands to validate this stage of your build.
      # test_commands: [echo 'running tests', make test]
    - name: prod
      requires_approval: true
```

You can run integration tests also at each stage using the test_commands in the stage yml configuration.


![Screen Shot 2020-11-25 at 8.03.53 AM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1606309477902/apn8brtVF.png)

The app is shipped to the production environment after approval. 🚚🚀

## Conclusion

🚀 AWS Copilot supercharges your application - one cli tool to set up infrastructure, build your application with many services, setting up the pipeline to automate release, monitor the status of stack and application, and with add-ons.  

AWS Copilot v1.0 is officially released. See this announcement.

%[https://twitter.com/nathankpeck/status/1330979599614959620]


In this article, we walked through how to set up the automated deployments of your applications. The source code of this demo is [here](https://github.com/ksivamuthu/copilot-ecs-dotnet-core-demo/tree/part-2).

What's next? In the upcoming articles of this series, we are going to complete our Coffee Shop Demo application by adding Order Service and Barista Service with event-driven architecture. That also includes the steps how AWS copilot adds storage, access other AWS services, environment variables, and more.

%[https://hashnode.com/series/aws-copilot-ecs-ckhskugtp07r2s6s14k1l6s56]

> Give the world a smile each day 

> If you’ve liked this post please share and clap 👏. Also, if you have any questions, please ask in the comments. Thank you! 

> Please follow me on my [twitter](https://www.twitter.com/ksivamuthu), [linkedin](https://www.linkedin.com/in/ksivamuthu/) and [github](https://www.github.com/ksivamuthu) for more articles/demos on cloud native, containers and mobile/web apps.


