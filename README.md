# QPet Website Development Project 

 
## 1.	BRIEF DESCRIPTION
### 1.1 WHO IS QPET?
QPet is a South African packaging manufacturer producing PET preforms, jars, closures, and bottles for industries including edible oils, food, water, chemicals, and carbonated beverages. The company operates across Pietermaritzburg, Johannesburg, and Cape Town.

We are collaborating with QPet to develop a modern, public-facing website that provides a professional online presence for the company and makes its product and company information easily accessible to prospective and existing clients.

The website is designed to provide a structured product catalogue, company information, branch details, and an online enquiry system. The solution is supported by a database, allowing product information and enquiries to be managed efficiently while providing a foundation that can be expanded as QPET's future digital needs evolve.

The website is focused on providing information and facilitating communication with QPet rather than functioning as an e-commerce platform. It therefore does not include online shopping, customer accounts, order management, or payment processing.


---------------------

## 2.	MAIN FEATURES 

| Feature | Description | 
|---|---|
| **Product Catalogue** | •	Browse QPET's available products. <br/> •	Filter and explore product listings.<br/> •	View approved product information and images. |
| **Contact Us** | •	Submit enquiries and questions through the website.<br/> •	Enquiries are stored in the database and routed to the designated QPET sales contact. |
| **Customer Reviews** | •	Users can submit reviews through the website. |
| **About Us** | •	Provides information about QPET, including its background and operations |
| **Admin Dashboard** | •	Add and manage products.<br/> •	View submitted enquiries.<br/> •	View and manage submitted reviews. |

---------------------

## 3.	TECHNOLOGY STACK 
**Frontend:** ASP.NET Core MVC, HTML, CSS, JavaScript  <br/>
**Backend:** C#, ASP.NET Core, Entity Framework Core <br/>
**Database:** Microsoft SQL Server, Entity Framework Core  <br/>
**Testing:** Unit testing and integration testing     <br/>
**Version Control:** Git/ GitHub                       <br/>
**CI/CD:** GitHub Actions

---------------------

## 4.	OVERALL PROJECT STRUCTURE 
The solution is divided into two main projects:
### 4.1 QPet MVC - The main ASP.NET Core MVC web application. <br/>
It contains the user-facing website, application logic, database integration, API controllers, authentication, and other core functionality. <br/>

_The MVC project follows the standard separation of responsibilities between: <br/>_
•	`Controllers` – Handle HTTP requests and application flow. <br/>
•	`Models` – Represent application data and validation rules. <br/>
•	`Views` – Provide the user interface.                       <br/>
•	`Services` – Contain reusable business and application logic.  <br/>
• `Data` – Handle database access, persistence, and supporting infrastructure. <br/>
•	`Security` – Handles authentication, authorisation, and security-related functionality.
### 4.2 QPet Tests
The solution includes a dedicated automated testing project containing unit and integration tests for key areas of the application, including: <br/>

•	Authentication and authorisation <br/>
•	Model validation                    <br/>
•	File upload security              <br/>
•	Database persistence                <br/>
•	API controllers and authorisation      <br/>
•	Integration testing                            <br/>

These tests help verify the functionality, security, and reliability of the application.

---------------------

## 5.	INSTALLATION AND SETUP 

### 5.1	 REQUIREMENTS/ PRE-REQUISITES 
Before running the application, ensure that the following is installed on the local development machine:
-	Visual Studio 2026 
-	.NET 10 SDK 
-	A modern web browser (e.g. Google Chrome, Microsoft Edge, Firefox or Safari)
-	Git Large File Storage (LFS)
---------------------
### 5.2	CLONE THE REPOSITORY 

The project can be cloned directly from GitHub using Visual Studio without manually entering Git commands.

#### 5.2.1 INSTALL GIT LFS
The project uses Git Large File Storage (Git LFS) to manage large files within the repository. <br/>
Git LFS should be installed before cloning the repository to ensure that all required project files are downloaded correctly.
1.	Download and install the latest version of Git LFS suitable for your operating system: https://git-lfs.com/
2.	Once installed, restart Visual Studio if it was already open.
   
#### 5.2.2 CLONE USING GITHUB + VISUAL STUDIO
1.	Open the QPet project repository on GitHub.
2.	Click the green **Code** button.
3.	Select **HTTPS**.
4.	Copy the repository URL.
5.	Open Visual Studio.
6.	Select **Clone a repository**.
7.	Paste the repository URL into the Repository location field.
8.	Select the folder where you would like to save the project.
9.	Click **Clone**.
10.	Visual Studio will download the repository and open the project automatically.

#### 5.2.3 IF GIT LFS FILES ARE MISSING
If the project files do not appear correctly after cloning, Git LFS may need to be initialised and the required files downloaded manually.
1.	In Visual Studio, open View → Terminal.
2.	Type:
`git lfs install` <br/>
You should see a success message similar to: Git LFS initialized.
3.	Type:
`git lfs pull`
4.	Then type:
`git restore --source=HEAD :/`
5.	Close Visual Studio completely.
6.	Reopen Visual Studio and reopen the QPet solution.   <br/>
The required Git LFS files should now appear in the project.

Note: If an error pop-up appears while opening the repository, close the pop-up and complete the Git LFS steps above before attempting to run the application.

---------------------
### 5.3	RESTORE PROJECT DEPENDENCIES 
After cloning the repository, Visual Studio should automatically restore the required NuGet packages.
If the packages are not restored automatically:
1.	Open the project in Visual Studio.
2.	In Solution Explorer, right-click the solution.
3.	Select Restore NuGet Packages.
4.	Wait for the package restoration to complete.
5.	Ensure there are no package or dependency errors in the Error List.

---------------------
### 5.4	CONFIGURE THE DATABASE 
The application uses a database to store and manage the system's required data.  

After cloning the repository and completing the Git LFS setup, the local database must be created and configured before running the application.
To create or update the local database: 
1.	Open the QPet solution in Visual Studio.
2.	Select Tools → NuGet Package Manager → Package Manager Console.
3.	In the Package Manager Console, run:
`Update-Database`
4.	Wait for the command to complete successfully.
5.	The required database and database tables should now be created or updated.
   
Note: Ensure the correct project is selected as the Default Project in the Package Manager Console before running the command.

--------------------- 
### 5.5	 RUN THE APPLICATION 
To run the application using Visual Studio:
1.	Open the QPet solution in Visual Studio.
2.	Ensure the correct project is selected as the Startup Project.
3.	Select the **HTTPS** launch profile.
4.	Click the green **Run** button or press **Ctrl + F5**.
5.	The application will open in the default web browser.

---------------------
### 5.6	 RUN AUTOMATED TESTS 
The solution includes a dedicated test project containing unit and integration tests for the application's services, security, models, database persistence, infrastructure, and API controllers.

Automated tests can be run directly through Visual Studio.
1.	Open the QPet solution in Visual Studio.
2.	Select **Test** from the top menu.
3.	Select **Test Explorer**.
4.	Wait for Visual Studio to discover the available tests.
5.	Click **Run All Tests**.
6.	The results will be displayed in the Test Explorer window.

---------------------
## 6. HOW THE API WORKS 
The QPet backend uses ASP.NET Core API controllers to expose REST API endpoints.  <br/>
No external API service is used. <br/>
The API controllers receive HTTP requests and communicate with the application's service layer, which handles the business logic and database operations through Entity Framework Core.

_The API is organised into four main controllers:_ <br/>
•	`BranchesApiController` – handles branch information.  <br/>
•	`EnquiriesApiController` – handles customer enquiries.  <br/>
•	`ProductsApiController` – handles product information and administration.   <br/>
•	`ReviewsApiController` – handles customer reviews.  <br/>

The API currently provides 15 endpoints using standard HTTP methods such as:
`GET`, `POST`, `PUT`, `PATCH` and `DELETE`.

---------------------
## 7. IMPORTANT API ENDPOINTS 

The QPet backend currently exposes 15 API endpoints across 4 API controllers.

### Branches API

| Method | Endpoint | Description | Access |
|---|---|---|---|
| `GET` | `/api/branches` | Retrieves all QPet branches, including contact and location information. | Public |
| `GET` | `/api/branches/{id}` | Retrieves a specific branch using its branch ID. | Public |

### Enquiries API

| Method | Endpoint | Description | Access |
|---|---|---|---|
| `GET` | `/api/enquiries` | Retrieves all customer enquiries. | Admin |
| `GET` | `/api/enquiries/{id}` | Retrieves a specific customer enquiry. | Admin |
| `POST` | `/api/enquiries` | Creates a new customer enquiry and validates the selected branch. | Public |
| `PATCH` | `/api/enquiries/{id}/status` | Updates the status of an existing enquiry. | Admin |

### Products API

| Method | Endpoint | Description | Access |
|---|---|---|---|
| `GET` | `/api/products` | Retrieves all active products, including category, subcategory and primary image information. | Public |
| `GET` | `/api/products/{id}` | Retrieves a specific active product using its product ID. | Public |
| `POST` | `/api/products` | Creates a new product. | Admin |
| `PUT` | `/api/products/{id}` | Updates an existing product. | Admin |
| `DELETE` | `/api/products/{id}` | Deletes an existing product. | Admin |

### Reviews API

| Method | Endpoint | Description | Access |
|---|---|---|---|
| `GET` | `/api/reviews` | Retrieves all customer reviews. | Public |
| `GET` | `/api/reviews/{id}` | Retrieves a specific customer review. | Public |
| `POST` | `/api/reviews` | Creates a new customer review and validates the selected branch. | Public |
| `DELETE` | `/api/reviews/{id}` | Deletes an existing customer review. | Admin |

---------------------
## 8. AUTHENTICATION/ADMIN INFORMATION
Some API endpoints are restricted to authenticated administrators. 

These endpoints use the application's QPETAdmin cookie authentication scheme and require the user to have the Admin role.

_**Public Endpoints**_ <br/>
The following operations are publicly accessible: <br/>
•	Viewing branches  <br/>
•	Viewing products  <br/>
•	Submitting enquiries  <br/> 
•	Viewing reviews  <br/>
•	Submitting reviews <br/>

_**Admin-Only Endpoints**_ <br/>
Administrator authentication is required for:  <br/>
•	Viewing all enquiries  <br/>
•	Viewing individual enquiries  <br/>
•	Updating enquiry statuses  <br/>
•	Creating products  <br/>
•	Updating products  <br/>
•	Deleting products  <br/>
•	Deleting reviews

The API does not expose administrator passwords or other sensitive credentials in the repository. 

Administrator accounts are seeded through the application's database seeding process.

For security, actual login credentials have not been stored in or shared through this README.

---------------------
## 9. ENVIRONMENTAL VARIABLES REQUIRED 

The production application uses environment variables to keep configuration and sensitive information separate from the application source code.

The following environment variables are required for the production environment:

| Variable | Purpose |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | Specifies the application environment, such as `Production`. |
| `ASPNETCORE_URLS` | Defines the internal URL and port on which the ASP.NET Core application listens. |
| `ConnectionStrings__DefaultConnection` | Provides the connection string used to connect the application to the production SQL Server database. |
| `AdminSeed__Email` | Defines the administrator email used during the database seeding process. |
| `AdminSeed__Password` | Defines the administrator password used during the database seeding process. |
| `Storage__EnquiryAttachmentsPath` | Defines the persistent server directory used to store uploaded enquiry attachments. |

Production values are stored in protected server-side environment configuration and are not committed to GitHub.

Sensitive values, including database credentials, administrator passwords, SSH private keys and deployment credentials, are intentionally excluded from this README.

GitHub repository secrets are also used where sensitive values are required by the CI/CD deployment workflow.

---------------------
## 10. BRANCHING STRATEGY 

The project uses a GitHub branching strategy to allow team members to work on different features without directly affecting the main `master` branch.

* **`master`** – The main stable branch containing the approved version of the project.
* **Feature branches** – Team members create separate branches when developing or modifying specific features.
* **Pull Requests** – Completed changes are reviewed and tested before being merged into `master`.
* **Merging** – Only tested and working changes are merged into `master`.

This approach helps prevent unfinished or untested changes from being deployed to the production environment.

**Different feature branches:** <br/>
<img width="575" height="509" alt="image" src="https://github.com/user-attachments/assets/a7d743f5-d0f3-49d5-92db-8349a55a28f1" />

Figure 1: Proof of different branches being used

---------------------
## 11. CI/CD EXPLANATION 

The project uses **GitHub Actions** to automate the build, testing and production deployment process.

The CI/CD workflow runs when changes are merged into the `master` branch. Pull requests can be built and tested without deploying changes to the production server.

The production deployment process follows these steps:

```
Checkout Repository
        |
        v
Set Up .NET 10
        |
        v
Restore Dependencies
        |
        v
Build Solution
        |
        v
Run Automated Tests
        |
        v
Publish Linux Release
        |
        v
Package Application
        |
        v
Secure SSH Connection
        |
        v
Upload Release to Server
        |
        v
Deploy Application
        |
        v
Restart qpet.service
        |
        v
Run Production Health Check
```
Deployment only continues when the build and automated tests complete successfully.

A dedicated deployment account and protected GitHub repository secrets are used for production deployment rather than storing private credentials directly in the workflow file.

The deployment process also supports safer redeployment by retaining the previous production release. If the new application fails to start or does not pass the production health check, the previous release can be restored.

The `/health` endpoint is used to verify that the application has started successfully and can communicate with the production database.

**GitHub Actions:** <br/>
<img width="1247" height="732" alt="image" src="https://github.com/user-attachments/assets/ecec8756-8f2f-43bc-b1b4-9353cf910811" />

Figure 2: Proof of successful GitHub Actions

---------------------
## 12. HOSTING/DEPLOYMENT EXPLANATION 

The QPet application is deployed to a **HostAfrica Linux VPS** located in South Africa. A VPS was selected because the application requires a server environment capable of running the ASP.NET Core application, production database, APIs and persistent uploaded files.

The production environment consists of:

- **Ubuntu 22.04 LTS**
- **.NET 10 ASP.NET Core**
- **Microsoft SQL Server 2022 Express**
- **Nginx reverse proxy**
- **systemd application service**
- **Let's Encrypt SSL certificate**
- Persistent storage for product images and enquiry attachments

#### Production Architecture

```
User Browser
     |
   HTTPS
     |
     v
Nginx Reverse Proxy
     |
     v
QPET ASP.NET Core Application
(.NET 10 / qpet.service)
     |
     v
Microsoft SQL Server
QPETDatabase

Persistent Storage:
- Product Images
- Enquiry Attachments
```

Nginx handles public HTTPS traffic and forwards requests to the ASP.NET Core application, which runs internally on `127.0.0.1:5000`. SQL Server is not directly exposed to the public Internet.

The application database is named `QPETDatabase` and was created using the project's Entity Framework Core migrations. Production database operations were verified through the live website and API, including reading and writing products, branches, reviews and enquiries.

#### HTTPS and Security

The hosted application uses HTTPS through a **Let's Encrypt SSL certificate**. Nginx manages the external HTTPS connection and forwards requests to the application.

The server firewall allows only the required services:

```
22   SSH
80   HTTP
443  HTTPS
```

#### Persistent Storage

Uploaded files are stored outside the application deployment directory so that they are not lost when a new application version is deployed.

The following persistent directories are used:

```text
/var/lib/qpet/product-images
/var/lib/qpet/enquiry-attachments
```

These directories are separate from `/opt/qpet`, which contains the deployed application files.

#### Application Service

The ASP.NET Core application runs as a Linux `systemd` service named `qpet.service`.

The production server was verified to have the required services running:

* `qpet.service` – QPet application
* `nginx` – Web server and reverse proxy
* `mssql-server` – Production database server

#### Database Backup

A SQL Server backup of the production `QPETDatabase` was created and verified using `RESTORE VERIFYONLY` to confirm that the backup file could be successfully read.

#### Production Health Check

The application provides a `/health` endpoint that is used after deployment to confirm that the application has started successfully and can communicate with the production database.

A successful health check returns:

```json
{
  "status": "Healthy",
  "database": "Connected"
}
```

#### Final Production Verification

After deployment, the following functionality was verified:

* Public website loads successfully over HTTPS.
* Administrator authentication works correctly.
* Products, branches and reviews can be retrieved from the database.
* Enquiries can be submitted and managed by administrators.
* Product management operations work correctly.
* Uploaded files remain available after redeployment.
* API endpoints respond correctly.
* HTTPS and the SSL certificate function correctly.
* The application restarts successfully after deployment.
* The production database remains accessible to the application.


---------------------
## 13. HOSTED APPLICATION URL 

The QPet application is hosted in a controlled production/demo environment.

**Hosted Website:**
https://102-209-117-83.sslip.io/

**Admin Portal:**
https://102-209-117-83.sslip.io/Admin/Login

**Health Check Endpoint:**
https://102-209-117-83.sslip.io/health

The hosted website is password protected and requires the provided preview credentials to access the application (for testing purposes). Administrator access to the Admin Portal requires separate QPet administrator credentials.

**Login credentials are not included in this README for security reasons and are provided separately where required.**

As the hosted website and Admin Dashboard are password protected, the Admin Dashboard and its functionality are demonstrated in the project video. **Please refer to the YouTube Video Link in Section 14 to view the website and administrator functionality in action.**

The hosted application is a separate project environment and does not replace QPet's existing public website or domain.

---------------------
## 14. YOUTUBE VIDEO LINK
PowerPoint Presentation posted on YouTube:  

[![Watch the video](https://img.youtube.com/vi/lHch9V8LOxA/0.jpg)](https://youtu.be/lHch9V8LOxA)


## 15. KNOWN LIMITATIONS 
### Product Images
Product image upload and storage functionality has been implemented. However, the development team is still awaiting the final approved product images from QPET. The available images may therefore be replaced with the client's final images before the project is handed over.
### About Us Content
The About Us section currently contains general company information. The final, more detailed content is still pending from QPET and will be updated before the final project submission.


