﻿using BTL_KhaoSatOnline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly SurveyDbContext _context;

    public AccountController(SurveyDbContext context)
    {
        _context = context;
    }

    // ====== LOGIN ======

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu.";
            return View();
        }

        // Tạo claims cho người dùng đăng nhập
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Thêm dòng này
        };


        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Đăng nhập bằng cookie
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    // ====== REGISTER ======

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        // Kiểm tra username đã tồn tại chưa
        if (_context.Users.Any(u => u.Username == user.Username))
        {
            ViewBag.Error = "Tên đăng nhập đã tồn tại.";
            return View(user); // Giữ nguyên dữ liệu đã nhập nếu lỗi
        }

        // Lưu tài khoản vào DB
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Chuyển sang trang đăng nhập
        return RedirectToAction("Login");
    }

    // ====== LOGOUT ======

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
