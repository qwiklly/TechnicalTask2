using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Dishes = new SelectList(_context.Dishes.ToList(), "Id", "Name");
            return View();
        }

        public IActionResult ResultDishes()
        {
            var records = _context.MealRecords
                .Include(m => m.User)
                .Include(m => m.Dish)
                .OrderByDescending(m => m.DateTime)
                .Take(20)
                .ToList();

            ViewBag.Records = records;
            ViewBag.Message = TempData["MealMessage"];
            ViewBag.Message1 = "Последние 20 записей о приёмах пищи";

            return View();
        }

        [HttpPost]
        public IActionResult AddDish(string dishName)
        {
            if (_context.Dishes.Any(d => d.Name == dishName))
            {
                return Json(new { success = false, message = "Это блюдо уже кто-то когда-то ел" });
            }

            var dish = new Dish { Name = dishName };
            _context.Dishes.Add(dish);
            _context.SaveChanges();

            return Json(new { success = true, dishId = dish.Id, dishName = dish.Name });
        }

        [HttpPost]
        public IActionResult AddMeal(string name, string email, int dishId)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || dishId == 0)
            {
                return Json(new { success = false, message = "Все поля обязательны для заполнения" });
            }

            var user = _context.Users.FirstOrDefault(u => u.Name == name && u.Email == email);
            bool isNewUser = false;

            if (user == null)
            {
                user = new User { Name = name, Email = email };
                _context.Users.Add(user);
                _context.SaveChanges();
                isNewUser = true;
            }

            var mealRecord = new MealRecord
            {
                DateTime = DateTime.Now,
                UserId = user.Id,
                DishId = dishId
            };
            _context.MealRecords.Add(mealRecord);
            _context.SaveChanges();

            var todayCount = _context.MealRecords.Count(m => m.DishId == dishId && m.DateTime.Date == DateTime.Now.Date);
            var userDishCount = _context.MealRecords.Count(m => m.UserId == user.Id && m.DishId == dishId);

            string message;
            if (isNewUser)
            {
                message = $"{name}, мы рады приветствовать вас в нашем сообществе! Вы только что съели {_context.Dishes.Find(dishId).Name}! За сегодня {_context.Dishes.Find(dishId).Name} было съедено {todayCount} раз!";
            }
            else
            {
                message = $"{name}, с возвращением! Вы только что съели {_context.Dishes.Find(dishId).Name}! Всего вы съели {_context.Dishes.Find(dishId).Name} {userDishCount} раз! За сегодня {_context.Dishes.Find(dishId).Name} было съедено {todayCount} раз!";
            }

            TempData["MealMessage"] = message;
            TempData["IsNewUser"] = isNewUser;

            return Json(new { success = true, message });
        }
    }
}


