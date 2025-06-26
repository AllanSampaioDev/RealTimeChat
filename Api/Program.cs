var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:2020");

builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddDbContext<RealTimeChatDataContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var apllyMigrations = builder.Configuration.GetValue<bool>("APPLY_MIGRATIONS");
if (apllyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RealTimeChatDataContext>();
        dbContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( cors => 
    cors.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:8080") 
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<HubProvider>("/chat-hub");

app.MapGet("messages/{userId1}/{userId2}", async (RealTimeChatDataContext db, Guid userId1, Guid userId2) =>
{
    var messages = await db.Messages.Where(m => 
        (m.SenderUserId == userId1 && m.RecieverUserId == userId2) ||                                         
        (m.SenderUserId == userId2 && m.RecieverUserId == userId1)
    )
    .OrderBy(m => m.SendTime)
    .ToListAsync();

    return Results.Ok(messages);
});

app.MapGet("user/", async (RealTimeChatDataContext db) =>  await db.Users.ToListAsync());
app.MapGet("user/{id}", async (RealTimeChatDataContext db, Guid id) => await db.Users.FindAsync(id));
app.MapPost("user/", async ([FromServices]RealTimeChatDataContext db, [FromBody]UserCreateRequest request) =>
{
    var entity = await db.Users.FirstOrDefaultAsync(x => x.Name == request.Name);

    if (entity is not null)
        return Results.BadRequest("Já existe um usuário com esse nome!");

    var entityToDb = new User()
    {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Password = request.Password
    };

    db.Users.Add(entityToDb);
    await db.SaveChangesAsync();

    return Results.Ok();
});
app.MapDelete("user/{id}", async (RealTimeChatDataContext db, Guid id) =>
{
    var entity = await db.Users.FindAsync(id);

    if (entity is null)
        return Results.BadRequest("Usuário não encontrado!");

    db.Users.Remove(entity);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("login/", async ([FromServices] RealTimeChatDataContext db, [FromBody] LoginRequest request) =>
{
    var entity = await db.Users.FirstOrDefaultAsync(x => x.Name == request.Name);

    if (entity is null)
    {
        var entityToDb = new User()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Password = request.Password
        };

        entity = db.Users.Add(entityToDb).Entity;

        await db.SaveChangesAsync();
    }
    else if (entity.Password != request.Password)
        return Results.BadRequest("Login e senha incorretos!");

    var handler = new JwtSecurityTokenHandler();

    // Usei a chave como um Guid mas deve ser uma chave mais complexa e não deve vir hardcoded.
    // Usei dessa forma apenas para fins de demonstração.
    var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("7139B5E0-D7CE-4D76-9964-EFC8F741BB99"));

    var credentials = new SigningCredentials(
        secretKey,
        SecurityAlgorithms.HmacSha256Signature
    );

    var tokenDescriptor = handler.CreateToken(
        new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim("id", entity.Id.ToString()),
                    new Claim("name", entity.Name)
                ]
            ),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials
        }
    );

    var stringToken = handler.WriteToken(tokenDescriptor);

    var result = new LoginResponse()
    {
        Id = entity.Id,
        Name = entity.Name,
        Token = stringToken
    };

    return Results.Ok(result);
});

await app.RunAsync();
