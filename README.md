# Notification Service Assessment

This is a quick implementation of a notification service. The goal was to build a system that can send messages through different channels (SMS, Email, etc.) while handling provider failures gracefully.

## Features
* **Abstraction:** Used interfaces for providers so you can swap between Twilio, Amazon, etc., without changing the main logic.
* **Failover:** If the primary provider fails, the system automatically tries the next one in the list based on priority.
* **Retries:** If all providers are down, the message is saved to the database and a background worker tries to resend it every minute.
* **Configurable:** You can enable/disable providers or change their priority in `appsettings.json`.

## A few notes on the code
* Used a bit of **DDD (Domain-Driven Design)** to keep the core logic inside the `Notification` entity.
* Implemented the **Strategy Pattern** for the providers to make it easy to add new ones (like WhatsApp) later.
* The "Delayed Resend" requirement is handled by a standard .NET **BackgroundService**.

## How to run
1. Open the solution in Visual Studio.
2. Set the `Notification.Api` as the startup project.
3. Run the project.
4. The project uses **SQLite**, so it will automatically create a local `.db` file in the folder on the first run. No database setup is needed.

You can test the endpoints using the Scalar/Swagger page that opens up in the browser.
