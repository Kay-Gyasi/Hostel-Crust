using API.Data.Repository;
using Data_Layer.Data_Context;
using Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFirstUnitTests
{
    public class CategoryRepoTests
    {
        private readonly HostelContext db;

        public CategoryRepoTests(HostelContext db)
        {
            this.db = db;
        }

        [Fact]
        public void GetCategoryByNameShouldWork()
        {
            // arrange
            //Categories category = new Categories() { Title = "Cake" };
            string title = "Cakes";

            // act
            CategoryRepo categoryRepo = new(db);

            // assert
            Assert.ThrowsAsync<ArgumentException>(() => categoryRepo.GetCategoryByName(title));
        }
    }
}
