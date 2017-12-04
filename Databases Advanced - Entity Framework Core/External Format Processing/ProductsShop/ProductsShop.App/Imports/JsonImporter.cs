﻿namespace ProductsShop.App
{
    using Newtonsoft.Json;
    using ProductsShop.App.Imports;
    using ProductsShop.Data;
    using ProductsShop.Models;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class JSONImporter : Importer
    {
        private const string UsersJsonFilePath = "Imports/Json/users.json";
        private const string ProductsJsonFilePath = "Imports/Json/products.json";
        private const string CategoriesJsonFilePath = "Imports/Json/categories.json";

        public JSONImporter(ProductsShopContext context) 
            : base(context)
        {
        }

        // Install-Package Newtonsoft.Json

        public override void Import()
        {
            var users = DeserializeJson<User>(UsersJsonFilePath);
            var products = DeserializeJson<Product>(ProductsJsonFilePath);
            var categories = DeserializeJson<Category>(CategoriesJsonFilePath);

            this.AssignUsersToProductsw(users, products);
            var categoriesProducts = this.AddProductsToCategories(products, categories);

            using (this.Context)
            {
                this.Context.Users.AddRange(users);
                this.Context.Products.AddRange(products);
                this.Context.Categories.AddRange(categories);
                this.Context.CategoriesProducts.AddRange(categoriesProducts);

                this.Context.SaveChanges();
            }
        }

        protected override TModel[] DeserializeJson<TModel>(string usersJsonFilePath)
        {
            var jsonString = File.ReadAllText(usersJsonFilePath);
            var collection = JsonConvert.DeserializeObject<IEnumerable<TModel>>(jsonString)
                .ToArray();

            return collection;
        }
    }
}
