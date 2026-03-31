# Resilient Notification Dispatcher

A highly extensible, multi-channel notification system built with a focus on Domain-Driven Design (DDD) and system resilience. This service abstracts various messaging providers (SMS, Email, Push) and ensures message delivery through intelligent failover and background retries.

## Architectural Highlights

### Domain-Driven Design (DDD)
The core logic is encapsulated within a Rich Domain Model. Instead of simple data containers, the entities manage their own state and enforce business invariants.
* Separation of Concerns: The solution is divided into Domain, Infrastructure, and API layers to ensure the business logic remains independent of technical details such as specific databases or third-party APIs.
* Ubiquitous Language: The codebase uses industry-standard terminology such as Dispatcher, Channel, and Provider to match the business requirements.

### Strategy Pattern and Extensibility
Messaging providers are implemented using the Strategy Pattern to ensure high modularity.
* The system communicates with providers through the INotificationProvider interface.
* Adding a new provider (e.g., WhatsApp, Firebase) requires zero changes to the core dispatching logic; it simply requires a new class implementation and registration in the DI container.

### Resiliency and Failover Logic
* Provider Priority: The dispatcher respects a configurable priority list. If the primary provider is unavailable, the system automatically attempts delivery via the next available provider.
* Delayed Retries: If all configured providers fail, the system persists the message with an AwaitingRetry status. A background worker service periodically attempts to re-dispatch these messages to ensure eventual delivery.

### Configuration (Options Pattern)
The project utilizes the .NET Options Pattern to bind configuration directly to C# objects. This allows for enabling or disabling providers and adjusting priorities at runtime via appsettings.json without requiring code redeployment.

## Technical Stack
* Framework: .NET 9 (Web API)
* ORM: Entity Framework Core
* Database: SQLite (configured for zero-friction evaluation)
* Patterns: Strategy, Options, Dependency Injection, Background Tasks

## Production Readiness
* **Request Validation:** Utilizes FluentValidation to ensure data integrity at the API boundary, preventing malformed requests from reaching the service layer.
* **Structured Logging:** Integrated Serilog to provide searchable, high-fidelity logs, essential for diagnosing provider failovers and system monitoring.
* **Domain Integrity:** Implemented Value Objects (Recipient, MessageContent) to address Primitive Obsession and ensure that domain entities remain in a valid state throughout their lifecycle.
* **Resilience Pipeline:** Leverages Polly to manage transient faults through exponential backoff retries, ensuring stable communication with external messaging vendors.

## How to Run
1. Open the solution in Visual Studio 2022.
2. Set Notification.Api as the Startup Project.
3. Run the application.
4. The database is automatically created and migrated on startup.
5. Use the Scalar/OpenAPI UI to test the POST /api/notifications endpoint.

---
*Developed as a technical portfolio project focusing on C# Systems Engineering and Scalable Architecture.*
