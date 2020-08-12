# graphql-dotnet-globalization-demo
An example of implementation of graphql in .net core 3.1, with globalization included

Based on graphql-dotnet (https://github.com/graphql-dotnet/graphql-dotnet ), this solution implements some important features:

1-Auto types starting from dto classes included. See original library https://github.com/fenomeno83/graphql-dotnet-auto-types

2-Validation management, with some additional custom validation

3-Globalization on enums

4-General globalization management for error messages based on resources (english and italian are actually configured, but you can add other resource files)

5-Error management in case of exception in call, validation failed, or auth failed

6-Log4net included

7-Grouping on queries and mutations, so you can use several files (similar to web api controllers structure) to write specific query or mutation group

8-Example of custom auth management. Project is not configured with auth, but you can use extension methods inside queries and mutations (see commented examples) if you configure auth in startup.cs (see .net core 3.1 guides to do this)

9-Lifetime at resolver level. This prevent parallel execution error with entity framework dbcontext and connection open only at service startup (because singleton)

10-GraphiQL GUI. See readme.txt in case you use auth

11-Several common extension methods

12-Test project

13-Visual studio 2019 template, if you want generate the solution using your own namespaces
