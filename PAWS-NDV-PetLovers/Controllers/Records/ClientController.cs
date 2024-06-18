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
        var clientVm = new ClientVm
        {
            Owner = new Owner(), // Initialize Owner object
            Pets = new List<Pet>() // Initialize Pets list
        };

        return View(clientVm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClientVm clientVm)
    {
        if (!ModelState.IsValid)
        {
            return View(clientVm);
        }

        // Save owner to the database
        _context.Owners.Add(clientVm.Owner);
        await _context.SaveChangesAsync();

        // Assign the owner ID to the pets
        foreach (var pet in clientVm.Pets)
        {
            pet.ownerId = clientVm.Owner.id; // Assuming the OwnerId property in the Pet model represents the owner ID
            _context.Pets.Add(pet);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddPetPost(ClientVm clientVm)
    {
        clientVm.Pets = new List<Pet>(); // Initialize the list if it's null

        if (clientVm.Pet != null)
        {
            clientVm.Pets.Add(clientVm.Pet); // Add the new pet to the list

            clientVm.Pet = new Pet(); // Clear the current Pet object for the next input

        }
        return View("Create", clientVm);

        // ithink need nig database kay add naman kag pet

    }
}