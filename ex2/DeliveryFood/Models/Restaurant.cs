using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace DeliveryFood.Models
{
    public class Restaurant
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Dish> Dishes { get; set; }
    }
    public class Dish
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double Price { get; set; }
        public Restaurant Restaurant { get; set; }

    }

    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double NutritionalValue { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<PromoCode> PromoCodes { get; set; }
        public List<Dish> dishes { get; set; }
        public enum Role { user, admin}
    }

    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime EndTime { get; set; }
        public PromoCode(string code, int id)
        {
            Code = code;
            BeginningTime = DateTime.Today;
        }
    }

}
