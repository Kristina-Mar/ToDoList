# ToDoList

The original commit is the final project for the C# 3 Czechitas course, further commits are my own.

To Do List as a web application. Back-end includes 3 projects:
- Domain (Models and DTOs)
- WebApi (controller-based REST API)
- Persistence (DbContext and Repository connecting the project to a SQLite database using Entity Framework Core)

Front-end uses Blazor, HTML, CSS (and Bootstrap). The app includes pages to add and edit tasks, buttons to order and filter tasks and icons to edit, change task completion status and delete a task (includes a pop-up modal to check the delete operation).

The project includes unit (xUnit incl. database mocking with NSubstitute) and integration tests.

The application (front-end) runs locally on port 5001, the API is available on port 5000. The solution includes two dockerfiles and a docker compose file so that the application can run in Docker, also on ports 5001 and 5000.
