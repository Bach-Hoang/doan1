using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DTO
{
    public class Food
    {
        private int id;
        private string name;
        private float price;
        private int categoryID;
        public Food(int id, string name, int categoryID,float price )
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.CategoryID = categoryID;
        }
        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.CategoryID = (int)row["idcategory"];
        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public float Price { get => price; set => price = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
    }
}
