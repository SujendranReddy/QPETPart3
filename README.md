# QPet Website Development Project 

 
## 1.	BRIEF DESCRIPTION
### 1.1 WHO IS QPET?
QPet is a South African packaging manufacturer producing PET preforms, jars, closures, and bottles for industries including edible oils, food, water, chemicals, and carbonated beverages. The company operates across Pietermaritzburg, Johannesburg, and Cape Town.

We are collaborating with QPet to develop a modern, public-facing website that provides a professional online presence for the company and makes its product and company information easily accessible to prospective and existing clients.

The website is designed to provide a structured product catalogue, company information, branch details, and an online enquiry system. The solution is supported by a database, allowing product information and enquiries to be managed efficiently while providing a foundation that can be expanded as QPET's future digital needs evolve.

The website is focused on providing information and facilitating communication with QPet rather than functioning as an e-commerce platform. It therefore does not include online shopping, customer accounts, order management, or payment processing.


---------------------

## 2.	MAIN FEATURES 
### Product Catalogue
•	Browse QPET's available products. <br/>
•	Filter and explore product listings.<br/>
•	View approved product information and images.
### Contact Us
•	Submit enquiries and questions through the website.<br/>
•	Enquiries are stored in the database and routed to the designated QPET sales contact.
### Customer Reviews
•	Users can submit reviews through the website.
### About Us
•	Provides information about QPET, including its background and operations
### Admin Dashboard
•	Add and manage products.<br/>
•	View submitted enquiries.<br/>
•	View and manage submitted reviews.

---------------------

## 3.	TECHNOLOGY STACK 
**Frontend:** ASP.NET Core MVC, HTML, CSS, JavaScript  <br/>
**Backend:** C#, ASP.NET Core, Entity Framework Core <br/>
**Database:**                                        <br/>
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
The application uses a database to store and manage the systems required data, 
Before running the application, ensure that the required database server is available and that the database has been created and configured. 

[ add steps !!!!]

---------------------
### 5.5	CONFIGURE CONNECTION STRINGS 

---------------------

### 5.6	RUN MIGRATIONS

--------------------- 

### 5.7	 RUN THE APPLICATION 
To run the application using Visual Studio:
1.	Open the QPet solution in Visual Studio.
2.	Ensure the correct project is selected as the Startup Project.
3.	Select the **HTTPS** launch profile.
4.	Click the green **Run** button or press **Ctrl + F5**.
5.	The application will open in the default web browser.
---------------------
### 5.8	 RUN AUTOMATED TESTS 
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

---------------------

## 7. IMPORTANT API ENDPOINTS 

---------------------

## 8. AUTHENTICATION/ADMIN INFORMATION
(WITHOUT EXPOSING ACTUAL PASSWORDS) 

---------------------

## 9. ENVIRONMENTAL VARIABLES REQUIRED 

---------------------

## 10. BRANCHING STRATEGY 

---------------------

## 11. CI/CD EXPLANATION 

---------------------

## 12. HOSTING/DEPLOYMENT EXPLANATION 

---------------------

## 13. HOSTED APPLICATION URL 

---------------------

## 14. YOUTUBE VIDEO LINK

## 15. KNOWN LIMITATIONS 
### Product Images
Product images have not yet been fully implemented, as the development team is still awaiting the required images from QPET. This functionality is expected to be implemented before the final project submission.
### About Us Content
The About Us section currently contains general company information. The final, more detailed content is still pending from QPET and will be updated before the final project submission.


