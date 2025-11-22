using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DTO
{
    public class Menu
    {   
        private float totalPrice;
        private float price;
        private string foodName;
        private int count;
        public Menu(string foodName, int count,float price ,float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.totalPrice = totalPrice;
            this.price = price;
            
        }
        public Menu(DataRow row)
        {
              this.FoodName = row["name"].ToString();
                this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        public int Count { get => count; set => count = value; }
        public string FoodName { get => foodName; set => foodName = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
