# 🛍️ ECommerce Microservice Architecture

This project is a **complete eCommerce system** built using a **Microservice Architecture** with an **API Gateway**.  
It demonstrates how to design, build, and integrate independent services to create a scalable, secure, and high-performance online shopping platform.

---

## 🚀 Overview

Each microservice is responsible for a specific business function such as:
- **Product Service** – manages product catalog, inventory, and details  
- **Order Service** – handles order creation, tracking, and history  
- **User Service** – manages user registration, authentication, and profile  

The **API Gateway** acts as a single entry point that:
- Routes incoming requests to the appropriate microservices  
- Enforces **security policies** (authentication & authorization)  
- Implements **rate limiting** to prevent abuse  
- Applies **caching** to improve performance  
- Uses **retry strategies** for reliability in case of transient network errors  

---

## ⚙️ Key Features

✅ **Microservice Architecture** – loosely coupled services for better scalability  
✅ **API Gateway Integration** – centralized request handling and routing  
✅ **Rate Limiting** – protects backend services from excessive requests  
✅ **Caching Layer** – stores frequently accessed data to boost speed  
✅ **Retry Policy** – increases system resilience against temporary failures  
✅ **Independent Databases** – each service manages its own data storage  
✅ **Service Communication** – RESTful APIs or asynchronous messaging (optional)  

---

## 🧩 Tech Stack

| Layer | Technology |
|-------|-------------|
| Backend Framework | ASP.NET Core / .NET 8 |
| API Gateway | Ocelot |
| Database | SQL Server |
| Authentication | JWT Bearer Token |
| Communication | HTTP |
| Logging | Serilog |

---

## 🧠 What I Learned

After completing this project, I gained a deep understanding of:
- Designing **scalable microservice architectures**
- Implementing an **API Gateway** with Ocelot
- Managing **service-to-service communication**
- Applying **best practices for caching, retry, and rate limiting**
- Deploying multiple microservices in containers

---

## 🏗️ Next Steps

- Integrate message-based communication with **RabbitMQ or Kafka**  
- Add **Payment Service** and **Inventory Service**  
- Implement **Monitoring & Logging Dashboard** using Prometheus + Grafana  
- Deploy to **Kubernetes** for production-grade orchestration  

---

## 🎥 Source

This project was created while following the tutorial:  
**“Building a Complete eCommerce Microservice Architecture with API Gateway”**  

---

