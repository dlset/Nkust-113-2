using CemeteryWeb.Models;
using CemeteryWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CemeteryWeb.Controllers
{
    // Controllers/CemeteryController.cs
    public class CemeteryController : Controller
    {
        private readonly ICemeteryService _cemeteryService;

        public CemeteryController(ICemeteryService cemeteryService)
        {
            _cemeteryService = cemeteryService;
        }

        public async Task<IActionResult> Index()
        {
            var cemeteries = await _cemeteryService.GetAllCemeteriesAsync();
            return View(cemeteries);
        }

        public async Task<IActionResult> Details(string name)
        {
            var cemetery = await _cemeteryService.GetCemeteryByNameAsync(name);
            if (cemetery == null)
                return NotFound();

            return View(cemetery);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "�п�ܦ��Ī� CSV �ɮ�";
                    return RedirectToAction(nameof(Upload));
                }

                await _cemeteryService.UploadCemeteryDataAsync(file);
                var count = _cemeteryService.GetCount();
                TempData["Success"] = $"���\�W�� {file.FileName}�A�@ {count} ���O��";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"�W�ǥ���: {ex.Message}";
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateReport(string district)
        {
            ViewBag.Districts = (await _cemeteryService.GetAllCemeteriesAsync())
                .Select(c => c.District)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            ViewBag.SelectedDistrict = district;

            var cemeteries = await _cemeteryService.GenerateReportAsync(district);
            return View("Report", cemeteries);
        }
    }
}
