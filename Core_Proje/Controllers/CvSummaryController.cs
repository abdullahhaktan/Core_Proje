using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{

    public class CvSummaryController : Controller
    {
        public readonly ICvSummaryService _cvSummaryService;

        public CvSummaryController(ICvSummaryService cvSummaryService)
        {
            _cvSummaryService = cvSummaryService;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Summarize([FromForm] CvInput input)
        {
            if (string.IsNullOrEmpty(input?.CvText))
                return BadRequest("CV içeriği boş olmaz.");

            var summary = await _cvSummaryService.GetSummaryAsync(input.CvText);

            TempData["Summary"] = summary;

            return RedirectToAction("Index", "Default");
        }
    }
}