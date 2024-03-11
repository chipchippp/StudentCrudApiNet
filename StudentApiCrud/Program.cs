using StudentApiCrud;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentsApiDb>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/students", async (StudentsApiDb db) =>
    await db.Students.ToListAsync());

app.MapGet("/students/complete", async (StudentsApiDb db) =>
    await db.Students.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/students/{id}", async (int id, StudentsApiDb db) =>
    await db.Students.FindAsync(id)
        is Student toDo
        ? Results.Ok(toDo) : Results.NotFound());

app.MapPost("/students", async (Student st, StudentsApiDb db) =>
{
    db.Students.Add(st);
    await db.SaveChangesAsync();

    return Results.Created($"/students/{st.Id}", st);
});

app.MapPut("/students/{id}", async (int id, Student inputTodo, StudentsApiDb db) =>
{
    var todo = await db.Students.FindAsync(id);
    if (todo == null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/students/{id}", async (int id, StudentsApiDb db) =>
{
    if (await db.Students.FindAsync(id) is Student todo)
    {
        db.Students.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.Run();
