# Bored But Broke

**A full-stack web app for finding affordable things to do in the UK**, built with an ASP.NET Core
RESTful API and a Blazor frontend. It gives personalised recommendations based on the weather
forecast, your budget, your age group and the kinds of things you enjoy.

![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor-Interactive_Server-512BD4?logo=blazor&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)

The frontend and backend were first developed in two separate repositories. This repository
brings them together, with their commit history, so the whole project can be viewed in one place.
The team's original pull requests are still available in the repositories below.

- **Original frontend repository:** https://github.com/michh18/bored-but-broke-front-end
- **Original backend repository:** https://github.com/yumnaeltabal/Bored-But-Broke-back-end

## Demo

[![Bored But Broke demo video](https://img.youtube.com/vi/HPBQWKxFyMs/maxresdefault.jpg)](https://youtu.be/HPBQWKxFyMs)

## Overview

Bored But Broke started from a familiar problem: you want to get out of the house, but you don't
know what to do and you don't want to spend much. Enter a UK location, a date and time window, the
kinds of things you enjoy, an age group and a budget, and the app returns matching places nearby.

The forecast shapes the results. If rain or snow is expected for much of the time you have free,
only indoor activities are suggested. Each result opens a page with opening hours, a map and a link
to the venue's website. Anyone can search without an account, and signing up lets you save places
to a favourites list.

The app was built by a team of four on the Northcoders software engineering bootcamp in May and
June 2026.

## Tech stack

| Area              | Technologies                                                                                                                                                                                         |
| ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Backend           | C#, .NET 8, ASP.NET Core Web API (RESTful), Entity Framework Core, LINQ, ASP.NET Core Identity, cookie authentication, output caching, rate limiting, health checks, Swagger / OpenAPI (Swashbuckle) |
| Frontend          | Blazor Web App (Interactive Server), Razor components, HTML, CSS, JavaScript, Bootstrap, Leaflet                                                                                                     |
| Database          | SQL Server                                                                                                                                                                                           |
| External services | Yelp Fusion API, Geoapify Geocoding API, Open-Meteo, OpenStreetMap                                                                                                                                   |
| Testing           | NUnit, Moq, Shouldly                                                                                                                                                                                 |
| Collaboration     | Git, GitHub, feature branches, pull requests                                                                                                                                                         |

## Features

### Finding things to do

- **Weather-Aware Results** When 40% or more of the chosen hours are forecast to have drizzle,
  rain, snow or thunderstorms, only indoor activities are returned.
- **Interest Categories** Users pick from 12 interests, such as Arts & Culture, Outdoor Adventure
  and Food & Drink.
- **Age-Appropriate Suggestions** Results are filtered for under-18s, adults or families with
  children.
- **Budget and Distance** Budget maps to price levels from £ to ££££, and the search radius runs
  from 1 to 15 miles.
- **Up to 16 Days Ahead** The date picker allows today and the next 16 days, which is as far ahead
  as Open-Meteo forecasts.
- **Sub-Category Filters** After searching, results can be narrowed to specific sub-categories,
  such as Museums or Flea Markets, and to price levels without running another search.

### Place details

- **Place Pages** Each place shows its photo, rating, price level, categories, weekly opening hours
  and a link to its website.
- **Interactive Map** A Leaflet map with OpenStreetMap tiles marks the place's location.
- **Popular Places** The homepage links to a hand-picked selection of eleven well-known venues.

### Accounts and favourites

- **User Accounts** Users can register, sign in and sign out, and the navigation bar greets them by
  first name.
- **Favourites** Signed-in users can save a place and remove it again. Signed-out visitors who
  press save are asked to sign in first.
- **Favourites List** Saved places are listed newest first, with the same category and price
  filters as search results. Each user has their own list, kept between sessions.

## External API integrations

### Yelp Fusion

Finds businesses near the user's coordinates, filtered by category, radius and price level. It also
provides the full details, opening hours and photo for each place page.

### Geoapify

Turns a town, city or postcode into coordinates. Searches are biased towards Great Britain, and any
location outside the UK is rejected with a clear message. The coordinates it returns are passed
straight to Open-Meteo for the forecast.

### Open-Meteo

Provides the hourly weather codes for the chosen date, which decide whether outdoor activities are
included. It does not need an API key.

### OpenStreetMap

Supplies the map tiles shown on each place page through Leaflet.

## Authentication and security

- **Hashed Passwords** ASP.NET Core Identity stores accounts with hashed passwords and requires a
  unique email for each one.
- **Password Rules** Passwords need at least 8 characters, a digit and a capital letter. The Blazor
  forms check the same rules before sending a request.
- **Account Lockout** Repeated failed sign-ins lock the account using Identity's default limits, and
  the API responds with 429 Too Many Requests while it is locked.
- **Secure Cookie Sessions** Signing in sets an HttpOnly, Secure, SameSite=Lax cookie that lasts
  seven days and renews while in use.
- **Protected Endpoints** Favourites endpoints require a signed-in user. Unauthenticated calls
  receive 401 or 403 instead of being redirected to a login page.
- **Protected Pages** The frontend guards the favourites page with `AuthorizeRouteView` and
  `[Authorize]`.
- **Revalidating Sign-In State** A custom authentication state provider asks `/api/auth/me` who is
  signed in, checks again every 20 minutes, and treats any failure as signed out.
- **Rate Limiting** Place lookups are limited to 20 requests per IP address in a five-minute window.
  Rejected requests receive 429 with a `Retry-After` header.
- **CORS Policy** The API only accepts cross-origin requests from the frontend's origin.

## Backend engineering

- **RESTful Endpoints** Places, favourites and authentication each have their own controller, and
  responses use standard status codes such as 201 Created, 204 No Content and 409 Conflict.
- **Service-Layer Architecture** Controllers, services, repositories and API clients each sit behind
  an interface and are connected through dependency injection, keeping a clear separation of
  concerns.
- **Configurable Category Data** 286 Yelp categories are grouped in JSON files by interest, by
  indoor or outdoor, and by age group. The API combines the chosen sets with union and
  intersection, then sends a single query to Yelp.
- **Typed HTTP Clients** Yelp and Geoapify each have a typed `HttpClient` with a 20-second timeout.
  A failed connection becomes 502 Bad Gateway and a timeout becomes 504 Gateway Timeout. An
  upstream error keeps its original status code.
- **Consistent Error Responses** A global exception handler maps each domain exception to a status
  code. Validation failures use the same problem-details shape, and every error includes a trace
  ID.
- **Request Validation** Data annotations check search, registration and sign-in requests. The
  search endpoint also rejects an end time earlier than the start time.
- **Output Caching** Searches and place lookups are cached for 60 minutes. Each combination of the
  nine search parameters has its own cache entry.
- **Code-First Database** Entity Framework Core code-first migrations define the SQL Server schema
  through a `DbContext` built on ASP.NET Core Identity, and repositories query it with LINQ. A place
  is saved to the database the first time someone adds it to their favourites, and a unique index
  stops a user saving the same place twice.
- **Health Check** A `/health` endpoint calls Yelp, Geoapify and Open-Meteo and reports the status
  of each one as JSON.
- **API Documentation** Swagger UI, generated from the OpenAPI specification, describes the API's
  endpoints and lets you call them in the Development environment.

## Frontend engineering

- **Interactive Server Rendering** Blazor renders pages on the server and keeps them interactive
  over a SignalR connection, so every API call is made from the server.
- **Dynamic Rendering** Search results, place pages and favourites are rendered from API responses,
  with loading states while data is fetched.
- **Reusable Components** Place cards, the results list, the map, the theme toggle and the sign-in
  display in the navigation bar are separate Razor components, most with their own scoped CSS.
- **JavaScript Interop** The Leaflet map and the theme switcher are set up from C# through
  JavaScript interop.
- **Form Validation** The search form checks required fields and the time range before searching.
  The sign-in and registration forms use `EditForm` with data annotations, plus show/hide password
  toggles.
- **Dark Mode** A toggle saves the chosen theme in `localStorage` and applies it through a
  `data-theme` attribute.
- **Error Handling** Unknown routes show a custom 404 page. When a search fails, for example because
  a required parameter is missing, the page shows the error message returned by the API and a link
  back home.
- **Responsive Layout** On screens narrower than 768px, the homepage and place pages switch to a
  single column.
- **API Status Page** A `/health` page calls the API's health check and shows an overall summary,
  then a card for each of Yelp, Geoapify and Open-Meteo marked healthy or showing its error. If the
  API itself can't be reached, the page says so.

## Testing

The backend has an NUnit unit test project for the place search, using Moq for mocking and Shouldly
for assertions. The services and API clients that call Yelp, Geoapify and Open-Meteo sit behind
interfaces, so the tests replace them with mocks and run without network access or API keys.

- **Place Service** `PlaceService` returns the places Yelp finds and marks results as indoor-only
  when the weather service reports bad weather. It builds the Yelp query from a search, narrowing
  an interest's categories by weather and age group and turning a budget into a range of price
  levels. Errors from the Yelp client are passed on rather than hidden.
- **Places Controller** `PlacesController` returns 200 OK with the service's results, whether or
  not any places were found.
- **Cancellation** Both layers pass the request's `CancellationToken` on to the calls they make.

The tests run with `dotnet test` from the repository root.

## Team collaboration

This project was built collaboratively as part of the Northcoders Software Engineering Bootcamp and
followed a team-based development approach:

- Using Git and GitHub for version control, with feature branches and pull requests
- Working through merge conflicts during concurrent frontend development
- Adjusting project scope based on API limitations and external dependencies
- Redistributing tasks when team availability changed during development
- Maintaining communication to ensure consistent progress toward MVP delivery

## Challenges and learning

Several real-world engineering challenges came up during the project:

- **Validating locations:** the original plan used only Open-Meteo and Yelp. Partway through, we
  realised user locations needed checking to make sure they were in the UK, so we integrated
  Geoapify. It also returned coordinates in the format Open-Meteo needed.
- **Unreliable external services:** Open-Meteo went down three times during development and
  testing, which meant no place results could be returned. This is why the app has a health check
  for all three APIs.
- **Inconsistent API responses:** external API response formats were not always consistent, so the
  response models handle missing fields safely.
- **API constraints:** some requirements had to change because of API limits and pricing.
- **Merge conflicts:** several contributors worked on the same frontend components at the same time.
- **Changing availability:** the workload had to be redistributed when team availability changed.

These challenges helped strengthen practical development skills, particularly communication,
adaptability, and delivering a working product under real constraints.

## Future improvements

- **Sorting and Distance** Let users sort places and show how far each place is from the location
  they entered.
- **More Results** Yelp returns at most 50 results per request, so a single search could make
  several API calls to offer a larger selection of places.
- **Social Features** Show how many users have favourited a particular place.
- **Account Management** Let users edit their profile, delete their account and recover a forgotten
  password.
- **Richer Place Details** Add attributes such as accessibility, wheelchair access, dog-friendliness,
  WiFi availability and noise levels.
- **Broader Test Coverage** Extend the unit tests to favourites, authentication and the API clients,
  and add tests for the Blazor frontend.

## Key takeaways

This project demonstrates the ability to:

- Build RESTful APIs using ASP.NET Core
- Integrate multiple third-party APIs into one feature
- Implement secure authentication and authorisation
- Store and protect user credentials securely
- Develop personalised, weather-based recommendations
- Work with location and weather data
- Model and query relational data with Entity Framework Core and SQL Server
- Build an interactive frontend with Blazor and reusable Razor components
- Apply service-layer architecture and write clean, maintainable code
- Collaborate effectively within a development team
