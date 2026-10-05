<p align="center">
  <img src="https://readme-typing-svg.herokuapp.com/?color=FFC72C&size=45&center=true&vCenter=true&width=1000&lines=NOOSHAPP;Restaurant+%26+Loyalty+Platform" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Architecture-Two--Tier%20MVC%20%2B%20API-FFC72C?style=flat-square&logoColor=black" />
  <img src="https://img.shields.io/badge/Framework-.NET%209-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Frontend-ASP.NET%20Core%20MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/API-ASP.NET%20Core%20Web%20API-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Database-SQLite-003B57?style=flat-square&logo=sqlite&logoColor=white" />
  <img src="https://img.shields.io/badge/ORM-EF%20Core-512BD4?style=flat-square" />
  <img src="https://img.shields.io/badge/Auth-Firebase-FFCA28?style=flat-square&logo=firebase&logoColor=black" />
  <img src="https://img.shields.io/badge/Docs-Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black" />
  <img src="https://img.shields.io/badge/PWA-Manifest%20%2B%20Service%20Worker-5A0FC8?style=flat-square&logo=pwa&logoColor=white" />
  <img src="https://img.shields.io/badge/Security-Defence%20in%20Depth-6f42c1?style=flat-square" />
  <img src="https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white" />
  <img src="https://img.shields.io/badge/Cloud-Azure%20Web%20Apps-0078D4?style=flat-square&logo=microsoftazure&logoColor=white" />
  <img src="https://img.shields.io/badge/Container-Docker-2496ED?style=flat-square&logo=docker&logoColor=white" />
  <img src="https://img.shields.io/badge/Transport-HTTPS%2FTLS-00A98F?style=flat-square" />
</p>

---

## Demonstration Video

<p align="center">
  <a href="https://www.youtube.com/watch?v=YOUR_VIDEO_ID">
    <img src="https://img.shields.io/badge/YouTube-Watch%20Demo-red?style=for-the-badge&logo=youtube&logoColor=white"/>
  </a>
</p>

> Click the badge above to watch a full walkthrough of the NooshApp application, covering the digital menu, rewards and QR flow, staff and admin tools, catering and careers.

---

## Main Project Link
<p align="center">
  <a href="https://github.com/YOUR-ORG/YOUR-MAIN-REPOSITORY">
    <img src="https://img.shields.io/badge/GitHub-Repository-black?style=for-the-badge&logo=github&logoColor=white" />
  </a>
</p>

## Backup Project Link

<p align="center">
  <a href="https://github.com/YOUR-ACCOUNT/YOUR-BACKUP-REPOSITORY">
    <img src="https://img.shields.io/badge/GitHub-Repository-white?style=for-the-badge&logo=github&logoColor=black" />
  </a>
</p>

---

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A [Firebase](https://firebase.google.com/) project with **Email/Password** sign-in enabled
- A Firebase **service-account key** (needed by the API to verify customer tokens)
- *(Optional)* An SMTP account for careers notification emails
- *(Optional)* [Docker](https://www.docker.com/) for container builds

### Installation

 **Clone, configure and run**
   ```bash
   git clone <your-repository-url>
   cd NooshApp

   # 1. API (terminal 1) - creates and seeds the SQLite database on first start
   dotnet run --project NooshApp.Api
   #    -> http://localhost:5021   (Swagger UI: /swagger, health check: /health)

   # 2. Website (terminal 2)
   dotnet run --project NooshApp.Web
   #    -> https://localhost:7137
  ```

### Configuration

Secrets must **never** be committed. Provide them through [user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) locally or environment variables on the host (`:` becomes `__`).

| Setting | Project | Purpose |
|---------|---------|---------|
| `ConnectionStrings:DefaultConnection` | API | SQLite file (default `Data Source=NooshAppApi.db`) |
| `Firebase:ServiceAccountKeyBase64` or `Firebase:ServiceAccountPath` | API | Credentials used to verify Firebase ID tokens |
| `Staff:ScanPin` | API | PIN staff use to scan QR codes and redeem rewards |
| `Admin:ApiKey` | API | Key protecting every `/api/admin` endpoint |
| `Smtp:Host` / `Port` / `Username` / `Password` / `FromEmail` | API | Careers notification emails |
| `BusinessContact:CareersEmail` | API | Recipient of job applications |
| `SelfBaseUrl` | API | Public API URL used in seeded menu image links |
| `ApiBaseUrl` | Web | URL of the API |
| `Firebase:ApiKey` / `AuthDomain` / `ProjectId` | Web | Public Firebase web configuration for browser sign-in |

> Change the default staff PIN and admin key before any real deployment.

---

## 1. System Overview

NooshApp is a restaurant website and loyalty platform for the Noosh brand. Customers can browse a live-searchable digital menu with allergen and spice information, find the nearest store for delivery, sign up for rewards and earn points in store using a QR code. The platform also provides catering enquiry and careers application forms and can be installed as a progressive web app (PWA). (Microsoft, 2024)

The system supports four main user types: **Visitors and Customers**, **Staff**, **Administrators** and **Job Applicants**. Customers use the menu and rewards dashboard, staff scan QR codes and redeem rewards at the till, administrators manage menu items, reward rules and the points rate, and applicants submit their CV through the careers form.

| Feature | Description |
|---------|-------------|
| Digital menu | Live search, category filters, allergen and spice icons |
| Rewards | Firebase sign-in, points dashboard (balance, history, rewards list) |
| QR points | Customer shows a 2-minute single-use QR; staff enter the sale amount and scan |
| Staff tools | Scan to award points, redeem a reward for a customer |
| Admin panel | Menu CRUD with image upload, reward rules, points-per-rand setting |
| Catering | Enquiry form with a confirmation reference |
| Careers | CV upload, keyword scoring and email notification to the owner |
| Delivery links | Geolocation picks the nearest store (Haversine distance) |
| PWA | Web manifest and service worker |

Security is a core requirement because the platform handles customer identities, loyalty points that act as a stored value, uploaded CVs and personal data. NooshApp therefore applies several controls at different layers rather than relying on a single mechanism. (OWASP, 2021)

---

## 2. System Architecture

### 2.1 Two-Tier Architecture

NooshApp is split into two ASP.NET Core 9 applications. **NooshApp.Web** is a server-rendered MVC website (Razor views, Bootstrap and vanilla JavaScript). **NooshApp.Api** is a REST API that contains the business rules and data access. The website calls the API over HTTP using typed HTTP clients, and the browser never calls the API directly. (Microsoft, 2024)

This separation keeps secrets such as the staff PIN, admin key and API address on the server side, allows the rewards logic to be reused by other clients in future, and keeps each application focused on a single responsibility.

### 2.2 System Components

| Layer | Component | Responsibility |
|-------|-----------|----------------|
| Presentation | Razor views, Bootstrap, JavaScript, PWA | Render pages, filter the menu, scan QR codes, Firebase sign-in |
| Web controllers | `NooshApp.Web/Controllers` | Handle browser requests, check the session, choose the view |
| API clients | `NooshApp.Web/Services` | Typed `HttpClient` classes that call the API and attach credentials |
| API controllers | `NooshApp.Api/Controllers` | REST endpoints, routing, status codes |
| Security filters | `NooshApp.Api/Auth` | `FirebaseAuthFilter`, `StaffPinFilter`, `AdminKeyFilter` |
| Services | `NooshApp.Api/Services` | Points rules, QR generation, CV scoring, email, admin logic |
| Repositories | `NooshApp.Api/Repositories` | Query and persist entities through EF Core |
| Data | `ApplicationDbContext` and SQLite | Ten entities, migrations and seed data |
| External | Firebase Auth, SMTP server, Azure, GitHub Actions | Identity, email, hosting and automation |

### 2.3 Backend Architecture

The API uses a layered structure: **Controller → Service → Repository → DbContext → SQLite**. Controllers bind the request and choose the HTTP status code, services contain the business rules, and repositories keep Entity Framework Core queries out of the rules. Every layer depends on interfaces supplied by constructor dependency injection, which keeps responsibilities clear and makes the services testable. (Microsoft, 2024)

Data crosses the API boundary as DTOs so that clients only receive and send the fields they are meant to. Expected business failures, such as an expired QR code, are returned as a result object that the controller maps to an HTTP 400 response.

### 2.4 System Boundaries

The main application boundary contains the MVC website, the REST API, the security filters, the business logic and the SQLite database.

External services include Firebase Authentication, the SMTP server used for careers emails, GitHub and GitHub Actions, and the Azure Web Apps used for hosting. These services support the application but are not part of its codebase.

HTTPS/TLS forms the secure communication boundary between users and the application. The authentication filters in the API then decide whether each request is valid and allowed.

---

## 3. Request Flow

A normal request begins when a user interacts with the website, for example opening the menu, signing in, generating a QR code or submitting a catering request. The browser sends the request to the MVC website over HTTPS.

The MVC controller checks the session where the page requires a login and then calls a typed API client. The client sends an HTTP request to the REST API and attaches the credential required by that endpoint: the Firebase ID token for customers, the staff PIN for staff actions, or the admin key for admin actions. Because the API is called from the server, these credentials are never exposed to browser scripts.

When the request reaches the API, the relevant security filter runs before the controller action. `FirebaseAuthFilter` verifies the customer token, `StaffPinFilter` checks the staff PIN, and `AdminKeyFilter` checks the admin key. If a check fails, the filter stops the request with an HTTP 401 response and the action never runs.

After the check succeeds, the controller calls the service. The service applies the business rules, for example checking that a QR token is unused and unexpired before awarding points, and uses repositories and EF Core to read or write SQLite. The result travels back through the controller as JSON, the MVC controller turns it into a view or a JSON response, and the browser displays it.

| Step | Layer | Example: staff scans a QR code |
|------|-------|--------------------------------|
| 1 | Browser | Staff enter the sale amount; the scanner reads the QR token |
| 2 | Web controller | `StaffController.SubmitScan` reads the PIN from the session |
| 3 | API client | `StaffApiClient.ScanAsync` calls `POST api/rewards/scan` with `X-Staff-Pin` |
| 4 | Security filter | `StaffPinFilter` validates the PIN |
| 5 | API controller | `RewardsApiController.Scan` calls the service |
| 6 | Service | `RewardsService.RedeemScanTokenAsync` validates the token and calculates points |
| 7 | Repository | `ScanTokenRepository` and `PointsRepository` update SQLite |
| 8 | Response | `PointsResultDto` returns the message and new balance to the scanner page |

This flow ensures that security checks run before protected functionality, and that each layer has a single responsibility.

---

## 4. Authentication and Access

### 4.1 Registration

Customers register in the browser using the Firebase JavaScript SDK. Firebase receives the email address and password, enforces its own password rules, and stores the credentials. NooshApp's own servers never receive or store customer passwords, and there is no password table in the database. (Firebase, 2024)

A customer record is created in the NooshApp database the first time the customer generates a QR code. It stores the email address, which is unique, and no password information.

### 4.2 Login

During login the browser signs the customer in with Firebase and receives a Firebase **ID token**, a signed JSON Web Token (JWT). The browser posts the token to the MVC website, which stores it in a server-side session. The browser only holds the encrypted session cookie, which is marked `HttpOnly`. (Firebase, 2024)

### 4.3 Token Verification

The ID token is not trusted by the website. Each time a protected API endpoint is called, `FirebaseAuthFilter` reads the `Authorization: Bearer` header and asks the Firebase Admin SDK to verify the token. The SDK checks the signature, the issuer, the audience and the expiry.

The API then reads the **email address from the verified token** and stores it for the request. Services use this email, never one supplied by the client, so a customer cannot access another customer's points by changing a request. The token contains only identity claims and no passwords. (Auth0, 2024)

### 4.4 Staff and Admin Access

Staff and administrators do not have individual accounts. Staff actions require the shared staff PIN in the `X-Staff-Pin` header, and every `/api/admin` endpoint requires the admin key in the `X-Admin-Key` header. Both values come from configuration, never from code.

The website verifies a PIN or key by making a harmless call to the API and checking the response, so the secret is stored only in the API configuration. After a successful check, the value is kept in the server-side session and forwarded as a header on each request.

### 4.5 Authentication Summary

| User | Mechanism | Verified by | Sent as |
|------|-----------|-------------|---------|
| Customer | Firebase email and password, ID token | `FirebaseAuthFilter` | `Authorization: Bearer <token>` |
| Staff | Shared PIN | `StaffPinFilter` | `X-Staff-Pin` |
| Admin | Shared key | `AdminKeyFilter` | `X-Admin-Key` |
| Visitor | None | Public endpoints only | n/a |

Authentication proves who or what is calling. The tiers above then decide what each caller may do. For example, a valid customer token does not allow staff or admin actions, and a customer can only ever see the account that belongs to the verified token.

---

## 5. Security Architecture

### 5.1 Credential Handling

Customer passwords are handled only by Firebase. The application stores no passwords, which removes password storage as an attack target and means a database leak does not expose customer passwords. Staff and admin secrets are read from configuration and compared by the API filters. (OWASP, 2021)

### 5.2 Access Control

Access is separated into three tiers: customer token, staff PIN and admin key. Each tier is enforced on the server by a dedicated filter attached to the relevant endpoints, so hiding a button in the browser cannot grant access. Customer data is scoped by the email in the verified token, which acts as an ownership check at the resource level. (OWASP, 2021)

### 5.3 QR Code Integrity

Loyalty points are protected by the design of the QR flow. The QR code contains only a random token. The token is stored with the customer, expires after **2 minutes** and can be used **once**. A screenshot of an old QR code is therefore useless, and staff must enter the sale amount at the time of scanning.

### 5.4 Data Integrity

Points are stored as an append-only **ledger**: every earn or redeem is a new row and the balance is the sum of the rows, so there is no stored balance that can drift. Unique database indexes prevent duplicate customer emails, duplicate QR tokens and duplicate receipt claims. Menu items and reward rules use **soft delete** so history is preserved.

### 5.4 Input Validation

Forms are validated on the website using data annotations and jQuery validation, and server-side with `ModelState`, for example required fields, email and phone formats, guest-count range and a rule that a catering date cannot be in the past. Uploads are restricted by file type and size: CVs must be PDF or DOCX up to 5 MB, supporting documents are limited to three files, and menu images must be JPG or PNG up to 5 MB. (Microsoft, 2024)

### 5.5 Output Encoding and CSRF

Razor views HTML-encode output by default, which reduces the risk of cross-site scripting. The careers and catering forms use anti-forgery tokens with `[ValidateAntiForgeryToken]` to protect against cross-site request forgery.

### 5.6 HTTPS/TLS and Cookies

HTTPS redirection and HSTS are enabled on the website so browsers use encrypted connections. The session cookie is `HttpOnly`, and the encryption keys that protect session and anti-forgery cookies are persisted so they survive restarts. HTTPS protects data in transit, while the Firebase token and filters establish identity. (Cloudflare, 2024)

### 5.7 Secure Error Handling

The website uses an exception handler and a custom "not found" page outside Development, so stack traces and internal details are not shown to visitors. Authentication failures return a short message and an HTTP 401 status without describing how the check works.

### 5.8 Secrets Management

Secrets such as the SMTP password, Firebase service-account key, staff PIN and admin key are provided through user-secrets or environment variables and must not be stored in source control. The API supports supplying the Firebase key as a Base64 environment variable for hosts that cannot upload files.

---

## 6. Security Rationale

The controls were chosen according to the risks of handling customer identities, stored-value loyalty points and uploaded personal documents.

| Security Control | Purpose |
|------------------|---------|
| Firebase Authentication | Removes password storage and handles sign-in |
| Server-side token verification | Proves the customer token is genuine and unexpired |
| Email taken from the verified token | Prevents customers acting on other accounts |
| Staff PIN and admin key filters | Restrict till and admin functions to authorised people |
| Single-use, 2-minute QR token | Prevents reuse and replay of loyalty codes |
| Unique indexes and ledger design | Protects data integrity and gives an audit trail |
| Input validation and upload limits | Rejects invalid or oversized input |
| Razor output encoding | Reduces cross-site scripting |
| Anti-forgery tokens (forms) | Reduces cross-site request forgery |
| HTTPS, HSTS, `HttpOnly` cookies | Protects data and sessions in transit |
| Server-side calls to the API | Keeps secrets out of the browser |
| Environment-based secrets | Keeps credentials out of source control |

These controls provide defence in depth: several mechanisms act at different stages of a request, so the system does not depend on a single control to protect sensitive functionality.

---

## 7. DevOps / CI-CD Overview

NooshApp uses Git branching and a GitHub Actions pipeline so that changes are built, checked and deployed in a repeatable way instead of by hand. (GitHub, 2024)

**Version control.** Work is done on feature branches, grouped by area (`backend/...` and `frontend/...`), which are merged into `develop` for integration and staging. `main` is the production release branch.

**Pipeline.** A push to `main` (or a manual dispatch) starts the workflow: the runner sets up .NET 9 and restores packages, the solution is built, the automated tests are executed, and the result is deployed. A failing build or test stops the pipeline so that a broken API endpoint does not reach the live site.

**Deployment.** The workflow signs in to Azure using **OpenID Connect (OIDC)**, so no long-lived Azure password is stored in the repository, and publishes both applications as zip packages to Azure Web Apps. (Microsoft, 2024)

| Stage | Description |
|-------|-------------|
| 1. Push to `main` | Commit or manual dispatch triggers the workflow |
| 2. Build | Ubuntu runner, .NET 9 restore and build |
| 3. Test | Automated tests run before artifacts are created |
| 4. OIDC login | Passwordless sign-in to Azure |
| 5. Deploy | `NooshApp.Api` and `NooshApp.Web` published to Azure Web Apps |

**Containers.** Each project includes a multi-stage Dockerfile: the .NET SDK image restores and publishes, and only the published output is copied into the smaller ASP.NET runtime image. The project file is copied first so the package restore layer is cached. (Docker, 2024)

   ```bash
   docker build -t nooshapp-api ./NooshApp.Api
   docker build -t nooshapp-web ./NooshApp.Web
   ```

**Operations.** Configuration is supplied through environment variables so that the same build runs in every environment, and the API exposes a `/health` endpoint for monitoring.

---

## Team

| Name | Role | Focus |
|------|------|-------|
| Taytem Pillay | Front End Software Developer | Page styling, design systems, Razor templates |
| Caleb K. Naidoo | Front End Software Developer | Web MVC views, PWA integration, mobile UX |
| Reece M. Moodley | Back End Software Developer | Core REST API, auth filters, middleware |
| Perez Seth Roy | Back End Software Developer | Loyalty ledger engine, file upload, CV scoring |

---

<p align="center">
  INSY7315 · Vision X Tech · 2026
</p>
