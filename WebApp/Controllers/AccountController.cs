using Infrastructure.Contexts;
using Infrastructure.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, WebAppContext context) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly WebAppContext _context = context;

    public async Task<IActionResult> Details()
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);


        var viewModel = new AccountDetailsViewModel
        {
            BasicInfo = new AccountBasicInfo
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Biography = user.Biography,
            },
            AddressInfo = new AccountAddressInfo
            {
                AddressLine_1 = user.Address?.AddressLine_1!,
                AddressLine_2 = user.Address?.AddressLine_2!,
                PostalCode = user.Address?.PostalCode!,
                City = user.Address?.City!,

            }
        };

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateBasicInfo(AccountDetailsViewModel model)
    {
        if (TryValidateModel(model.BasicInfo!))
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                user.FirstName = model.BasicInfo!.FirstName;
                user.LastName = model.BasicInfo!.LastName;
                user.Email = model.BasicInfo!.Email;
                user.PhoneNumber = model.BasicInfo!.PhoneNumber;
                user.UserName = model.BasicInfo!.Email;
                user.Biography = model.BasicInfo!.Biography;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = "Basic information updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong! Unable to update account information.";
                }
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Something went wrong! Unable to update account information.";
        }

        return RedirectToAction("Details", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAddressInfo(AccountDetailsViewModel model)
    {
        if (TryValidateModel(model.AddressInfo!))
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);
             
            try
            {
                if (user != null)
                {
                    if (user.Address != null)
                    {
                        user.Address.AddressLine_1 = model.AddressInfo!.AddressLine_1;
                        user.Address.AddressLine_2 = model.AddressInfo!.AddressLine_2;
                        user.Address.PostalCode = model.AddressInfo!.PostalCode;
                        user.Address.City = model.AddressInfo!.City;
                    }
                    else
                    {
                        user.Address = new AddressEntity
                        {
                            AddressLine_1 = model.AddressInfo!.AddressLine_1,
                            AddressLine_2 = model.AddressInfo!.AddressLine_2,
                            PostalCode = model.AddressInfo!.PostalCode,
                            City = model.AddressInfo!.City
                        };
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["StatusMessage"] = "Adress information updated successfully.";
                }
            } 
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong! Unable to update address information.";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Something went wrong! Unable to update address information.";
        }

        return RedirectToAction("Details", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && file != null && file.Length != 0)
            {
                var fileName = $"p_{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads", "profiles");
                var filePath = Path.Combine(directoryPath, fileName);


                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                user.ProfileImage = fileName;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to upload image: File or user is null.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while uploading the image: " + ex.Message;
            
        }

        return RedirectToAction("Details", "Account");
    }

}
