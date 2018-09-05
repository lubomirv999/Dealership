# Dealership
Team Project for Kodar Internship Program - September 2018

## General Requirements
#### Dealership application should use the following technologies, frameworks and development techniques:
* Implement using **ASP.NET Core Framework**.
* View Engine for UI - **Razor**.
* Database - **Microsoft SQL Server**.
* Database Access - **Entity Framework Core**.
* Responsive design - **Bootstrap**.
* Users and Roles - **ASP.NET Identity System**.
* Error handling and data validation - **Client** and **Server** side.
* Source control - **Github**

## Database Structure And Logic
### Table:

#### Cars 
* Id
* Manufacturer
* Model
* Year Of Production 
* Body Type 
* Condition 
* Type Of Transmission 
* Travelled Distance 
* Horse Power 
* Color
* Euro Standart
* Sale Description
* Engine Type 
* Price
* Image Urls

## Business Logic of the application
### Three types of users:
* Anonymous (not registered and not logged in user)
* Logged in users
* Administrator

### Anonymous users are able to
* Register
* Login
* View All Cars

### Logged in users are able to
* Buy car

### Administrators are able to
* CRUD on Cars

### Deadline
* Project should be completed before 26 September 2018
