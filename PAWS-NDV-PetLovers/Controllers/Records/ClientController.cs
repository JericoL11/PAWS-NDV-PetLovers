using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ClientController : Controller
{
    private readonly PAWS_NDV_PetLoversContext _context;

    public ClientController(PAWS_NDV_PetLoversContext context)
    {
        _context = context;
    }



    public async Task<IActionResult> Index()
    {
        return View(await _context.Owners.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Owner owner)
    {
        // Save owner to the database
        _context.Owners.Add(owner);
                // Assign the owner ID to the pets
            
                await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}

