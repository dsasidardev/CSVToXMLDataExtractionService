# CSVToXMLDataExtractionService
Installation steps:
Clone the solution to your local folder
Run the following nuget packages to install the required libararies:

Install-Package Microsoft.Extensions.Configuration.Json -Version 5.0.0
Install-Package Microsoft.Extensions.Configuration -Version 5.0.0
Install-Package Topshelf
Install-Package Microsoft.Extensions.Configuration.Binder -Version 5.0.0

Overview of this service:
As per the requirement: This service can be run from Visual Studio or can published to SCM where it will run on any designated server.  There are folder and timings configurations in the appsettings.json file where user can modify the folder locations of CSV/xml and service schedule timings etc. Upon running the service, it will generate XML file on defined folder location.  
