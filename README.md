# Employee Shift Management

This is a sample application for scheduling working shifts of employees given a specific set of rules.


## Specification

The application caluculate a shift schedule consisting of two shifts per day, based on the total number of employees and total number of days provided from the user. Holidays (Saturday, Sunday) are considered off days.

## Rules

- Employees can do at most one-half day shift in a day.
- Employees cannot have two afternoon shifts on consecutive days. 
- Each employees should complete one whole day of support in the scheduling period. 
- If an employee works on two consecutive days, they are eligible to get two days exemption.
- Scheduling should be started from the first working day of the upcoming week.
- Employees should not work on Holidays (Optional)

## How to run

This project is consisted of an ASP 2.1 Core solution for backend and an Angular 6 project for frontend UI. The communication between projects are based on standard REST Service.

```
POST http://{SERVER_IP}/api/schedule HTTP/1.1

Content-Type: application/json

{
	TotalEmployees: 10, 
	FirstShiftEmployee: 1, 
	SecondShiftEmployee: 2, 
	TotalDays: 14, 
	HolidaysOff: true
}
```

### Run backend

Open the solution located in `Sources/BackEnd/CompanyX.EmployeeShiftManagement.sln` in Visual Studio 2017. Press F5 to debug the project. UI is build as static HTML files and have been put on wwwroot folder, so there is no need to run Angular frontend to get the demo running.

The project has two options for runnig:

#### Run backend in Docker

To run the backend in Docker you will need to install [Docker for Windows](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/visual-studio-tools-for-docker?view=aspnetcore-2.1)

Right click on docker-compose and select Set As Startup Project. Press F5 to debug the project using Docker. 

Please ensure that Docker daemon is up and running. 

If Docker option is not present in the Launch Button, please remove the docker-compose project, right-click on `CompanyX.EmployeeShiftManagement` project, select Add and then click on Docker Support.

#### Run backend in IIS Express

If Docker is not present in the machine, the project can be run via IIS Express. Simply right-click on docker-compose project and then click on Unload Project menu Item. Now the default debugger is set to IIS Express. Press F5 to debug the project using IIS Express.
 
### Run frontend

UI is statically built and have been put on wwwroot folder on the Visual Studio Solution, so **there is no need to run Angular frontend to get the demo running**.

But for the development purposes, fronend source can be built by node.js 8.9.4 and npm 5.6.0. Goto the terminal and execute these tasks:

```
cd Sources/FrontEnd
npm install
npm start
```

Please ensure to **configure backend server's IP into the Angular application by setting `SERVICE_URL` property in `settings.ts` file.**





