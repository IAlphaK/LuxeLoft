using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxeLoft.Data;
using LuxeLoft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace LuxeLoft.Controllers
{
    [Authorize]
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PropertiesController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Properties/SearchProperties
        public IActionResult SearchProperties()
        {
            ViewBag.Location = new SelectList(new List<string> { "Islamabad", "Karachi", "Faisalabad", "Lahore", "Multan", "Other" });
            ViewBag.Type = new SelectList(new List<string> { "House", "Apartment", "Vacant Land", "Office", "Shop", "Factory", "Plaza", "Other" });
            return View();
        }

        // POST: Properties/SearchProperties
        [HttpPost]
        public async Task<IActionResult> SearchProperties(string location, string type, int minPrice, int maxPrice)
        {
            var properties = _context.Properties
                .Include(p => p.Owner)
                .Where(p => (location == null || p.Location == location) &&
                            (type == null || p.Type == type) &&
                            p.Price >= minPrice &&
                            (maxPrice == 0 || p.Price <= maxPrice));

            ViewBag.IsSearchResult = true; // Add this line
            return View("Index", await properties.ToListAsync());
        }


        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Properties.Include(p => p.Owner);
            ViewBag.IsSearchResult = false; // Add this line

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyListings()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized(); // or return to the login page if the user is not authenticated
            }

            var userProperties = _context.Properties
                .Include(p => p.Owner)
                .Where(p => p.OwnerID == currentUser.Id);

            return View(await userProperties.ToListAsync());
        }

        // GET: Properties/Details/5
        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Features) // Make sure to include the Features
                .FirstOrDefaultAsync(m => m.PropertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            // Now, you should be able to access owner's properties
            var owner = property.Owner;
            if (owner != null)
            {
                // Set the UserName and PhoneNumber in ViewData
                ViewData["UName"] = owner.UserName;
                ViewData["PNum"] = owner.PhoneNumber;
            }

            return View(property);
        }


        public async Task<IActionResult> DetailsBuyer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.Features) // Make sure to include the Features
                .FirstOrDefaultAsync(m => m.PropertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            // Now, you should be able to access owner's properties
            var owner = property.Owner;
            if (owner != null)
            {
                // Set the UserName and PhoneNumber in ViewData
                ViewData["UName"] = owner.UserName;
                ViewData["PNum"] = owner.PhoneNumber;
            }

            return View(property);
        }


        // GET: Properties/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Set ViewBag properties for Location and Type dropdowns
            ViewBag.Location = new SelectList(new List<string> { "Islamabad", "Karachi", "Faisalabad", "Lahore", "Multan", "Other" });
            ViewBag.Type = new SelectList(new List<string> { "House", "Apartment", "Vacant Land", "Office", "Shop", "Factory", "Plaza", "Other" });
            return View();
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Location,Type,ThumbnailFile")] Property property, string[] features)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            _context.Users.Attach(currentUser); // This ensures EF knows the user already exists.
            property.OwnerID = currentUser.Id;

            if (currentUser == null)
            {
                return NotFound("User not found");
            }
            //property.Owner.About = currentUser?.About ?? "Default About";
            //property.Owner.Name= currentUser.Name = currentUser?.Name ?? "Default Name";

            if (!ModelState.IsValid)
            {
                // Log the error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // You can view these in the debug output window while debugging
                System.Diagnostics.Debug.WriteLine("Model Errors: " + string.Join(", ", errors));

                // ... the rest of your code for redisplaying the form
            }

            if (ModelState.IsValid)
            {
                // Save the thumbnail image and set the Thumbnail path
                if (property.ThumbnailFile != null && property.ThumbnailFile.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(property.ThumbnailFile.FileName);
                    string extension = Path.GetExtension(property.ThumbnailFile.FileName);
                    property.Thumbnail = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Img/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await property.ThumbnailFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(property);
                await _context.SaveChangesAsync();

                // Handle the features
                foreach (var featureName in features.Where(f => !string.IsNullOrWhiteSpace(f)))
                {
                    var feature = new Feature { PropertyID = property.PropertyID, Feature_Name = featureName };
                    _context.Add(feature);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyListings)); // Replace 'MyListings' with your actual listing page action method
            }
            // If we got this far, something failed, redisplay form
            // Re-populate ViewBag for Location and Type if needed
            ViewBag.Location = new SelectList(new List<string> { "Islamabad", "Karachi", "Faisalabad", "Lahore", "Multan", "Other" });
            ViewBag.Type = new SelectList(new List<string> { "House", "Apartment", "Vacant Land", "Office", "Shop", "Factory", "Plaza", "Other" });
            return View(property);
        }


        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .Include(p => p.Features)
                .Include(p => p.Owner)
                .ThenInclude(o => o.Properties) // Include user's properties if needed
                .FirstOrDefaultAsync(m => m.PropertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || property.OwnerID != currentUser.Id)
            {
                // If the current user is not the owner, redirect or show unauthorized message
                return Unauthorized();
            }

            // Pre-populate ViewBag for Location, Type, and Features
            ViewBag.Location = new SelectList(new List<string> { "Islamabad", "Karachi", "Faisalabad", "Lahore", "Multan", "Other" });
            ViewBag.Type = new SelectList(new List<string> { "House", "Apartment", "Vacant Land", "Office", "Shop", "Factory", "Plaza", "Other" });
            ViewBag.Features = property.Features.Select(f => f.Feature_Name).ToList();

            return View(property);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyID,OwnerID,Title,Description,Price,Location,Type,ThumbnailFile")] Property property, string[] features)
        {
            if (id != property.PropertyID)
            {
                return NotFound();
            }

            var existingProperty = await _context.Properties
               .Include(p => p.Owner)
               .Include(p => p.Features)
               .FirstOrDefaultAsync(m => m.PropertyID == id);

            if (existingProperty == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    // Update the properties of existingProperty with the values from property
                    existingProperty.Title = property.Title;
                    existingProperty.Description = property.Description;
                    existingProperty.Price = property.Price;
                    existingProperty.Location = property.Location;
                    existingProperty.Type = property.Type;

                    // Update thumbnail if needed
                    if (property.ThumbnailFile != null && property.ThumbnailFile.Length > 0)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(property.ThumbnailFile.FileName);
                        string extension = Path.GetExtension(property.ThumbnailFile.FileName);
                        existingProperty.Thumbnail = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Img/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await property.ThumbnailFile.CopyToAsync(fileStream);
                        }
                    }

                    var existingFeatures = _context.Features.Where(f => f.PropertyID == property.PropertyID);
                    _context.Features.RemoveRange(existingFeatures);

                    foreach (var featureName in features.Where(f => !string.IsNullOrWhiteSpace(f)))
                    {
                        var feature = new Feature { PropertyID = property.PropertyID, Feature_Name = featureName };
                        _context.Add(feature);
                    }

                    // Now save the existingProperty since it's the tracked entity
                    _context.Update(existingProperty);
                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyListings));
            }
            return RedirectToAction(nameof(MyListings));

        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Properties.FindAsync(id);
            if (@property != null)
            {
                _context.Properties.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyID == id);
        }
    }
}
