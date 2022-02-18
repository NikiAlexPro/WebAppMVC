using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;


var builder = WebApplication.CreateBuilder(args);

//Строка для подключения из файла кофигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShopProducts}/{id?}");

//app.MapGet("/", (ApplicationContext db) => db.Client.ToList());
//app.MapGet("/", async (ApplicationContext db) =>
//    {
        
//        //Client client = new Client() { id=Guid.NewGuid(), FirstName = "Никита", LastName = "Мехедов", Patronymic = "Алексеевич"};
        
//        //DetailClient detailClient1 = new DetailClient() { Email = "qwe@rambler.ru", BirthDate = DateTime.Now, Phone = "89102375928", Client = client };
//        //DetailClient detailClient2 = new DetailClient() { Email = "qwe@yandex.ru", BirthDate = DateTime.Now, Phone = "89103395882" , Client = client };
        
//        //await db.Client.AddAsync(client);
//        //await db.DetailClient.AddAsync(detailClient1);
//        //await db.SaveChangesAsync();
//        return db.DetailClient.Include(u => u.Client).ToList();
        
//    }
//);


app.Run();
