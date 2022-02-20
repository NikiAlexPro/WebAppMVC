using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        ApplicationContext context;
        public HomeController(ApplicationContext db)
        {
            context = db;
        }





        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailClient detailClient)
        {
            //IQueryable<DetailClient> detailClients = context.DetailClient.Include(c => c.Client);
            Client? client = await context.Client.FirstOrDefaultAsync(client =>
            client.FirstName == detailClient.Client.FirstName &&
            client.LastName == detailClient.Client.LastName &&
            client.Patronymic == detailClient.Client.Patronymic);
            if(client != null)
            {
                detailClient.Client = client;
                await context.DetailClient.AddAsync(detailClient);
                await context.SaveChangesAsync();
            }
            else
            {
                detailClient.Client.id = Guid.NewGuid();
                client = detailClient.Client;
                await context.Client.AddAsync(client);
                await context.DetailClient.AddAsync(detailClient);
                await context.SaveChangesAsync();

            }
            return RedirectToAction("Clients");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            IQueryable<DetailClient> detailClients = context.DetailClient.Include(c => c.Client).AsNoTracking();
            DetailClient dc = detailClients.FirstOrDefault(c => c.id == id);
            if (dc != null)
                return PartialView(dc);
            else
                return NotFound(dc);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DetailClient detailClient)
        {
            IQueryable<DetailClient> ListdC = context.DetailClient.Include(c => c.Client).AsNoTracking();
            DetailClient sourcedc = ListdC.FirstOrDefault(c => c.id == detailClient.id);
            if(sourcedc.Client.Compare(detailClient.Client))
            {
                //context.Client.Update(detailClient.Client);
                context.DetailClient.Update(detailClient);
                await context.SaveChangesAsync();
            }
            else
            {
                detailClient.Client.id = Guid.NewGuid();
                Client newClient = detailClient.Client;
                detailClient.Client = newClient;
                await context.Client.AddAsync(newClient);
                context.DetailClient.Update(detailClient);
                await context.SaveChangesAsync();
            }
            //Если в процессе меняется Client(имя, фамилия, отчество), то нужно создать новый client 
            //и привязать к нему текущий detailClient(поменять Client_id).
            //Иначе - просто сохранить изменения в detailClient

            //if(ModelState.IsValid)
            return RedirectToAction("Clients");
        }


        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            IQueryable<DetailClient> detailClients = context.DetailClient.Include(c => c.Client).AsNoTracking();
            DetailClient dc = detailClients.FirstOrDefault(c => c.id == id);
            if (dc != null)
                return PartialView(dc);
            else
                return NotFound(dc);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            DetailClient detailClient = context.DetailClient.FirstOrDefault(c => c.id == id);
            if (detailClient != null)
            {
                context.DetailClient.Remove(detailClient);
                await context.SaveChangesAsync();
                return RedirectToAction("Clients");
            }
            else
                return NotFound(detailClient);
            
        }


        [HttpGet]
        public IActionResult Clients()
        {
            var clients = context.DetailClient.Include(c => c.Client).ToList();
            return View(clients);
        }

        [HttpGet]
        public IActionResult ShopProducts()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}