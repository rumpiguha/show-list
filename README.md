# .Net core console app to show sorted list

**Architectural Summary**

 This is VS2017 solution that runs on .NET Core 2.2, C3 7.1 and LINQ
 It consists of a Console App, a .NET Standard Library and Test Projects.
 It uses MSTest and Moq for unit testing.
 All unit tests are in Project "ShowPetsTest" project, when viewing the solution on VS2017

**Application Purpose**
The coding challenge is to build an app that consumes JSON data from an API and provides an alphabetically sorted list of all the cats with the gender of their owner. For example:

Male

*Angel
*Molly

Female

*Gizmo
*Jasper

**Notes**
To Test:
Download this solution to your machine and open the solution file (ShowCats.sln) with VS2017.
Once loaded on VS2017, press Ctrl-F5. This should launch a console app showing the results.
If the app is unable to connect to the People Web Service, it will show a friendly text message on the console.
