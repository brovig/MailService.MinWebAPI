using MailService.MinWebAPI;
using MailService.MinWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailSender, MailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to Mail Web Service!");

app.MapGet("/api/mails", async (DataContext context) =>
    await context.MailMessages.ToListAsync());

app.MapPost("/api/mails", async(DataContext context, MailMessage message, IMailSender mailSender) =>
{
    //send message method
    await mailSender.SendEmailAsync(message);

    message.CreationDate = DateTime.Now;
    if (String.IsNullOrEmpty(message.FailedMessage))
    {
        message.SendResult = true;
    }
    else
    {
        message.SendResult = false;
    }

    context.MailMessages.Add(message);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/api/mails/{id}", async (DataContext context, int id) =>
{
    var dbMessage = await context.MailMessages.FindAsync(id);
    if(dbMessage == null) return Results.NotFound("No message with this ID");
    context.MailMessages.Remove(dbMessage);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
