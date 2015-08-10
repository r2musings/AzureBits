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
                
                // hard-coded adding one thumbnail for now
                model.Thumbnails.Add(new Thumbnail { Width = 200, Height = 300 });
                
                await _imageService.AddImageToBlobStorageAsync(model);
                await _queueService.AddMessageToQueueAsync(model.Name, model);
            }

            return View("Index", model);
        }
    }
}