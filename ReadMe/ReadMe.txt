FilmReference Application

These files are provided in good faith but as-are: I believe but cannot guarantee that they are virus free. Likewise I believe that they are free of malicious code - I
certainly haven't written any! - but cannot guarantee it. The risk - which I believe to be non-existant - is yours and it is up to you to prove to your satisfaction
that they are ok.

General

There is a script to create the database with all tables, joins etc in the DbScript soltion folder. Run this first.

Ensure that the connection string in appsettings.json is correct for your SQL Server instance and run the application. This should load
the application, which at this point will obviously contain no data.

Create a Genre or two, then do the same for Studio and Director, as all of these are required when creating a film: you will not
be able to create a Film without these.

Films can be created without Actors as these can be added later.

Architecture

All classes relating to the database are contained in the DbClasses folder in the DataAccess project. These are all partial
so as to be able to separate business logic and validation via DataAnnotations. The Business partial classes all
have the [ModelMetadata(typeof(YourClassNameModelMetadata))] annotation and the ModelMetadata classes are in the obvious place.

Configuration is done by means of a separate ConfigurationClass for each DbClass and the configurations are called from
the context class - which is, in this case, FilmReferenceContext. This class can be found in the root of the DataAccess project.

Most PageModel classes in the FrontEnd project inherit from a custom class called FilmReferencePageModel, which in turn inherits
from PageModel. This does very little in this application other than provide the context. If it was required to perform
auditing on the creating and amendment of data FilmReferencePageModel is be an ideal place to trap the Id if the current user
and pass it to the context so that the Id will be accessible to any code you might want to write as an override to SaveChangesAsync
to automate the recording of the audit data.

(I can show this in action in another project if required).

Enjoy!

Feel free to make changes, suggestions, (constructive) criticisms and whatever else comes to mind. Please just bear in mind that
although I have been developing for twenty years or so I am still (relatively) new to .Net Core!

Julian Aburrow

March 2020