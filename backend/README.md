# 🌍 Bored But Broke API

An ASP.NET Core Web API built to power **Bored But Broke**, an application designed to help users discover affordable activities based on their location, the weather, personal preferences, age, and how far they're willing to travel.

Developed as a team project, the API integrates multiple third-party services to deliver personalised recommendations while providing secure user authentication and data management. The project focuses on clean architecture, maintainable code, and real-world API integration.

## ✨ Highlights

* Built RESTful API endpoints to support activity recommendations
* Integrated multiple external APIs to provide personalised results
* Implemented secure user authentication using cookies
* Stored user accounts with hashed passwords
* Developed a favourites system for saving activities
* Applied service-layer architecture and separation of concerns
* Organised activity categories through configurable JSON data
* Designed with maintainability in mind

## 🛠 Tech Stack

| Technology            | Purpose                        |
| --------------------- | ------------------------------ |
| C#                    | Core programming language      |
| ASP.NET Core Web API  | Backend framework              |
| Entity Framework Core | ORM and database access        |
| SQL Server            | Relational database            |
| Cookie Authentication | User authentication            |
| LINQ                  | Querying and data manipulation |
| OpenAPI / Swagger     | API documentation and testing  |
| JSON Configuration    | Activity category management   |

## 🌐 External API Integrations

### Open-Meteo

Used to retrieve the hourly forecast for the chosen date and time, and recommend indoor activities when rain, snow or storms are expected.

### Yelp Fusion API

Used to discover local businesses, attractions, and venues that match a user's preferences.

### Geoapify

Used to geocode the user's location into coordinates, so recommendations can be found within their chosen travel range.

## 🔐 Authentication & Security

The API uses cookie-based authentication to manage user sessions securely.

### Features

* User registration
* User login and logout
* Password hashing for secure credential storage
* Protected endpoints for authenticated users
* Session management using authentication cookies

## 📡 Core Functionality

### User Management

* Register new accounts
* Secure login and logout
* Retrieve the signed-in user's details

### Activity Recommendations

* Weather-aware recommendations
* Location-based suggestions
* Preference-driven results
* Age-appropriate activity filtering
* Travel distance filtering

### Favourites

* Save recommended activities
* Retrieve saved favourites
* Remove activities from favourites

## 🗄 Database Integration

The application uses SQL Server and Entity Framework Core to manage user data and application persistence.

### Entity Framework Core Features Used

* DbContext configuration
* Code-first migrations
* Entity tracking
* LINQ querying
* Create, read and delete operations
* Dependency injection integration

## 🏗 Architecture

The API follows a layered structure to encourage maintainability and separation of concerns.

### Design Principles

* Service layer architecture
* Dependency injection
* RESTful API design
* Separation of concerns
* Configurable JSON-based category management
* Clean and maintainable code practices

## 📚 Key Takeaways

This project demonstrates the ability to:

✅ Build RESTful APIs using ASP.NET Core

✅ Integrate multiple third-party APIs

✅ Implement secure authentication and authorisation

✅ Store and protect user credentials securely

✅ Build personalised, filter-based activity recommendations

✅ Work with location and weather-based data

✅ Apply service-layer architecture and clean coding principles

✅ Collaborate effectively within a development team

## 🚀 Future Improvements

* Enhanced recommendation algorithms
* User profile customisation
* Activity history tracking
* Advanced filtering options
* Improved caching and performance optimisation
* Additional third-party integrations
* Expanded activity categories

---

Bored But Broke was developed as a team project to explore real-world API integration, secure user authentication, and personalised activity recommendations while building a backend using modern .NET development practices.
