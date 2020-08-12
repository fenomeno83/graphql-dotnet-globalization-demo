Use automatic dto to graph types converter inside TypeCatalog file.
The rules to do this are:
-when a new input model is created, you need to create related type using GraphQLInputGenericType extension method and convention dto_name + InputType for main class and for all sub classes.
-when a new output model is created, you need to create related type using GraphQLGenericType extension method and convention dto_name + Type for main class and for all sub classes.

See existing examples in that file to understand.

Like new queries and mutations, you don't need to register types in Startup.cs, because they are autoamatically looped from assembly.


IMPORTANT!

We have implemented 2 approaches to manage scoped dbcontext lifetime inside singleton graphql middleware: 
-1 where dbcontext is injected at request lifetime, but this can fails in case of graphql parallel execution; 
-1 where a factory is injected, so you can instantiate dbcontext explicitly (in this case there is an example that shows how pass dbcontext between the various methods that works also alone)

If you need to use authorization, you need to configure startup.cs (see some .net core tutorial).
We have added some extension methods to validate for example scopes inside queries and mutations (see commented code in queries and mutations), but you can use the same approach to validate others entities.
In case of token based auth, to test graphql using graphi ui, you need to inject bearer token authentication, otherwise GUI will be in error.
To do this, you need:
-Install chrome extension called "ModHeader"
-Generate token, using for example a postman call
-Open ModHeader extension and choose "Authorization" as name and paste token as value
-Now you can start project and authorization header will be inject in all requests