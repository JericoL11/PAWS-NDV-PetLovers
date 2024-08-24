using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class ServicesController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public ServicesController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {

            var services = await _context.Services.ToListAsync();

            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("serviceId,serviceName,serviceCharge")] Services service)

        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            if (ServiceNameExist(service.serviceName))
            {
                ModelState.AddModelError("", $"Service Name {service.serviceName} already exists!");
                return View(service);
            }

            // Add the service to the database
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the appropriate action
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var serviceId = _context.Services.Find(id);

            if (serviceId == null)
            {
                return NotFound();
            }

            return View(serviceId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("serviceId,serviceName,serviceCharge")] Services service)
        {

            //matching the id

            if (id != service.serviceId)
            {
                return NotFound();
            }


            try
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceIdExist(service.serviceId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
        }

        #region == Functions ==
        public bool ServiceNameExist(string serviceName)
        {
            return _context.Services.Any(s => s.serviceName == serviceName);
        }

        public bool ServiceIdExist(int id)
        {
            return _context.Services.Any(s => s.serviceId == id);
        }

        #endregion


        [HttpGet]

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var find = _context.Services.Find(id);

            if (find == null)
            {
                return NotFound();
            }

            return View(find);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var findAsycn = await _context.Services.FindAsync(id);

            if (findAsycn == null)
            {
                return NotFound();
            }

            _context.Services.Remove(findAsycn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
