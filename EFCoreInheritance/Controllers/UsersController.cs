using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreInheritance.Data;
using EFCoreInheritance.DTOs;
using EFCoreInheritance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInheritance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ECommerceDBContext _context;
        public UsersController(ECommerceDBContext context)
        {
            _context = context;
        }
        // ---------------- CREATE USER (Unified Endpoint) ----------------
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            User user = dto.UserType.ToLower() switch
            {
                "customer" => new Customer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordHash = dto.PasswordHash,
                    PhoneNumber = dto.PhoneNumber!,
                    ShippingAddress = dto.ShippingAddress!,
                    BillingAddress = dto.BillingAddress!
                },
                "seller" => new Seller
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordHash = dto.PasswordHash,
                    BusinessName = dto.BusinessName!,
                    BusinessLicenseNo = dto.BusinessLicenseNo!,
                    BankAccountNumber = dto.BankAccountNumber!,
                    GSTNumber = dto.GSTNumber!,
                    PANNumber = dto.PANNumber!,
                    WarehouseAddress = dto.WarehouseAddress!,
                    SupportPhone = dto.SupportPhone,
                    IsVerified = dto.IsVerified ?? false
                },
                "admin" => new AdminUser
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordHash = dto.PasswordHash,
                    RoleName = dto.RoleName!,
                    Department = dto.Department!,
                    Permissions = dto.Permissions
                },
                _ => throw new Exception("Invalid User Type")
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new { Message = $"{dto.UserType} created successfully", UserId = user.Id });
        }
        // ---------------- GET ALL USERS ----------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var result = users.Select(u => new UserReadDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Username = u.Username,
                UserType = u.GetType().Name
            });
            return Ok(result);
        }
        // ---------------- GET BY TYPE ----------------
        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            IEnumerable<User> list = type.ToLower() switch
            {
                "customer" => await _context.Customers.ToListAsync(),
                "seller" => await _context.Sellers.ToListAsync(),
                "admin" => await _context.AdminUsers.ToListAsync(),
                _ => new List<User>()
            };
            return Ok(list);
        }
    }
}