using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;


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
            //Сортировка
            var products = context.ShopProduct.Include(c => c.DetailClient).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateShop()
        {
            var detailclients = context.DetailClient.Include(c => c.Client).ToList();
            var a = detailclients.Select(o => new SelectListItem
            {
                Value = o.id.ToString(),
                Text = o.Client.FirstName + " " 
                + o.Client.LastName + " " 
                + o.Client.Patronymic + " " 
                + o.Phone + " " 
                + o.Email + " " 
                + o.BirthDate.ToString()
            }); ;
            DetailClientListViewModel detailClientListViewModel = new DetailClientListViewModel()
            {
                //DetailClients = new SelectList(detailclients, "id", "Client.FirstName")
                DetailClients = new SelectList(a, "Value", "Text")
            };
            //Добавление detailclient с помощью выбора в списке
            //Добавление изображения
            
            return PartialView(detailClientListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShop(DetailClientListViewModel detailClientListViewModel)
        {
            var select_id = detailClientListViewModel.shopProduct.DetailClient_id;
            DetailClient? detailclient = await context.DetailClient.FirstOrDefaultAsync(dc => dc.id == select_id);
            if (detailclient != null)
            {
                detailClientListViewModel.shopProduct.id = Guid.NewGuid();
                detailClientListViewModel.shopProduct.DetailClient = detailclient;
                detailClientListViewModel.shopProduct.ScreenImage = ImageConverterToByte.ConvertToByte(detailClientListViewModel.FormFile);
                detailClientListViewModel.shopProduct.ImageFormat = ImageConverterToByte.ContentType(detailClientListViewModel.FormFile);
                await context.ShopProduct.AddAsync(detailClientListViewModel.shopProduct);
                await context.SaveChangesAsync();
                return RedirectToAction("ShopProducts");
            }
            else
                return NotFound(detailclient);
            
            //Найти detailClient по id
            //Обновить detailClient и добавить ShopProduct
            
        }

        [HttpGet]
        public IActionResult ImageShow(Guid id)
        {
            var imageFromDB = context.ShopProduct.FirstOrDefault(x => x.id == id);
            return PartialView(imageFromDB);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            var detailproducts = context.ShopProduct.Include(c => c.DetailClient).ThenInclude(c => c.Client).ToList();
            ShopProduct shopProduct = detailproducts.FirstOrDefault(x => x.id == id);
            return PartialView(shopProduct);
        }

        [HttpGet]
        public IActionResult DeleteShop(Guid id)
        {
            var detailproducts = context.ShopProduct.Include(c => c.DetailClient).ThenInclude(c => c.Client).ToList();
            ShopProduct shopProduct = detailproducts.FirstOrDefault(x => x.id == id);
            return PartialView(shopProduct);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShop(ShopProduct shopProduct)
        {
            if (shopProduct != null)
            {
                var findingShopProduct = await context.ShopProduct.FirstOrDefaultAsync(x => x.id == shopProduct.id);
                if (findingShopProduct != null)
                {
                    context.ShopProduct.Remove(findingShopProduct);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("ShopProducts");
            }
            else
                return NotFound();
            
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