using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AzureBits.Core.Models;
using AzureBits.Core.Services;

namespace AzureBits.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IQueueService<UploadedImage> _queueService;

        public HomeController(IImageService imageService,  IQueueService<UploadedImage> queueService)
        {
            _imageService = imageService;
            _queueService = queueService;
        }

        public ActionResult Index()
        {
            return View(new UploadedImage());
        }

        [HttpPost]
        public async Task<ActionResult> Upload(FormCollection formCollection)
        {
            var model = new UploadedImage();
            
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["uploadedFile"];
                model = await _imageService.CreateUploadedImage(file);
                await _imageService.AddImageToBlobStorageAsync(model);
                await _queueService.AddMessageToQueueAsync(model.Name, model);
            }

            return View("Index", model);
        }
    }
}


