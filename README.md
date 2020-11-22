# AWS Copilot - ECS - DotNet Core Demo

## Introduction

You've your application running in containers. What are the steps you are taking to deploy the containers? How much time does it take to create the infrastructure, pipelines to build, release, and operate production-ready containerized applications? Do we've any toolkit available for developers to accelerate the deployment of containerized applications in AWS ECS & Fargate? 

Yes. **AWS Copilot CLI** is a tool for developers to build, release, and operate production-ready containerized applications on Amazon ECS and AWS Fargate. From getting started, pushing to staging, and releasing to production, Copilot can help manage the entire lifecycle of your application development.

In this blog post, I'm going to walk through the steps of how AWS Copilot helps developers/customers focus on building the applications rather than setting up their infrastructure.

## Installing CLI

Install the AWS Copilot CLI in your Mac through Homebrew or Cloud 9 IDE

```bash
brew install aws/tap/copilot-cli
```

As well as, you'll need the basic tools to use AWS Copilot, such as - Docker Desktop, AWS Credentials with necessary permissions, your favorite code editor (VSCode), etc. Make sure that you have a default profile for your AWS credentials.  

Okay. The CLI is installed. What's next? It should be easy with few commands in your terminal. Before looking into our demo, let's go through a few concepts in Copilot that help us understand what's going on behind the scenes.

## Features

What are the features the containerized application requires to run?

- Network Resources - VPC, Subnets, Security Groups, etc
- ECS Cluster - Cluster of Containerized applications/services - Containers running on EC2  or use Fargate for serverless containers
- Load Balancer - To access the production environment from the internet.
- Internal Load Balancer - To access the non-production / QA environment from the internal network.
- Environments - Different environments to deploy your code.
- Container Registry - Registry for your application containers
- Pipeline - Pipeline to build, release your applications.

Pretty much, we need the above items to set up the production-grade applications. 

The AWS Copilot will take care of setting up the whole infrastructure that's required. The core concepts of AWS Copilot help you to set them up.

### Applications 
An Application is a collection of services and environments. It's a high-level product name. You can create multiple services under the application.

### Environments
You can deploy your service to the test environment and then deploy it to your production environment. The environment isolation is required to gate what features you want to release to your end customers. You can deploy multiple services in separate environments - the services will share the same network and cluster for your environment.

### Services
You can choose different service types when Copilot CLI asks whether your service is a back-end service or load balancer web service or the scheduled job for ephemeral workloads. 

## .NET 5 Application & Dockerfile

Now, let's start with the demo. I've created a simple Hello World .NET Core 5 API code and the dockerfile running the .NET 5 API code in the container.  Check out this repo. Now we are going to get it deployed to AWS using AWS Copilot.

%[https://github.com/ksivamuthu/copilot-ecs-dotnet-core-demo]

In the directory run, 

```bash
copilot init
```

Answer the questions the CLI is asking.
- the application name
- what type of service
- which docker file you like to use for your service.

![Screen Shot 2020-11-20 at 10.31.06 PM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1605929527151/dUYkntqL6.png)

Once you answered the questions, Copilot will start setting up the AWS infrastructure to manage your service. It's using Cloudformation to create and manage the infrastructure your application and services require.

## Deploy in ECS

Once Copilot finishes setting up the infrastructure to manage your app, you’ll be asked if you want to deploy your service to a test environment type yes.

Copilot sets up all the resources needed to run your service in the test environment. After the environment infrastructure is created, Copilot will build your docker image, push it to Amazon ECR, and start deploying to Amazon ECS.

![Screen Shot 2020-11-20 at 11.01.00 PM.png](https://cdn.hashnode.com/res/hashnode/image/upload/v1605931280344/q5b8Yyc-0.png)

Now, your service will be up and running on AWS Fargate.  You can create production or other environments using copilot commands.

## Conclusion

Copilot is doing the heavy lifting for us - the developers. So we can focus on the application. Copilot will take care of the building, pushing, and launch your container on AWS. It's easy, right. You can build an entire application infrastructure with microservices, load balancer, container registries. 

Okay. I hear from you. It's a simple service, and the deployment has been done from your computer. What about multiple services? How to do automatic deployment from Git repository? How to provision the storage? Can we access the logs of the container? How to monitor the applications? Let's see in detail in the next post. 

If you like this post, please do follow me, like, and comment. Your suggestions are welcome. And if you have any questions, please feel free to ask or reach me at my Twitter [ksivamuthu](https://twitter.com/ksivamuthu)


> Encourage one another and build each other up !!!
