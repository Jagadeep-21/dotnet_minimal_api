using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Tododb>(opt => opt.UseInMemoryDatabase("Todolist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();

app.MapGet("/todoItems", async (Tododb db) => await db.Todos.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, Tododb db) => await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound());
app.MapGet("/todolist/isComplete", async (Tododb db) => await db.Todos.Where(t => t.isComplete).ToListAsync());
app.MapGet("/", () => "Hello World!");

app.MapPost("/todoItems", async (Tododb db, Todo t) =>
{   
    System.Console.WriteLine("posting..........");
    db.Todos.Add(t);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{t.id}", t);



});

app.MapPut("/todoItems/{id}", async (int id, Tododb db, Todo t) =>
{
    var exist = await db.Todos.FindAsync(id);
    if (exist is null) return Results.NotFound();
    exist.name = t.name;
    exist.isComplete = t.isComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();

});
app.MapDelete("/todoItems/{id}", async (int id, Tododb db, Todo t) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        var exist = await db.Todos.FindAsync(id);
        if (exist is null) return Results.NotFound();
        db.Todos.Remove(t);
        await db.SaveChangesAsync();
        return Results.Ok(t);
    }
    return Results.NotFound();

});


app.Run();
