So I had this idea after watching a Pluralsight course by Alex Wolf titled *ASP.NET Core: The MVC Request Life Cycle*

When watching the section about the MVC Router, I thought that there had to be a different way to use the Router pipeline without having all the additional overhead of the MVC framework for generating an API only project.

I've always liked the style of creating a *descriptive* api, and I wanted to do something whereby you could do so in a fluent, strongly typed fashion. Also I wanted to get away from having to do repetitive boilerplate for every controller and action etc.

So here it is. At this point is is very raw and only has 2 Describers. A Custom Action and a Paged. There is a long way to go from here.
## Usage
So there is a sample project that is using both the Custom Action and the Paged Describers, but here is the general idea.

In the Configure Services you need to register the `ApiRouteDescriptor` service and it's backing datastore (In this example I am using EntityFrameworkCore with SQL Server) like so:
```
var connection = @"Server=(localdb)\mssqllocaldb;Database=ApiRouteDescriptorSample;Trusted_Connection=True;";
services.AddApiRouteDescriptor().AddEntityFrameworkDataStore<SampleContext>(o => o.UseSqlServer(connection));
```
You will need to create a class that implements the `ApiDefinition` class and *describe* your routes in the constructor
```
public class SampleApiDefinition : ApiDefinition
{
    public SampleApiDefinition()
    {
        this.Get["Root", "api"] = this.CustomAction<HomeResponder>();
        this.Get["People", "api/people"] = this.Paged<Person, PersonResource>("Name");
    }
}
```
Finally in the projects `Configure` method add the `ApiRouteDescriptor` to the pipeline.
```
app.UseApiRouteDescriptor(new SampleApiDefinition());
```
## Moving Forward
- I want to finish out the Paged describer to include stuff like links for each resource, paging information like next and previous page links etc. Add Fluent extensions to the describers to allow customization for things like Number of items per page, or query customization.
- Add more Describers. For other methods like POST, PUT, DELETE etc.
- Create a Model Mapper that allows customization of the resources in a fluent fashion.
- Add in other ideas like Authorization, Error handling
- Anything else that comes to mind.
