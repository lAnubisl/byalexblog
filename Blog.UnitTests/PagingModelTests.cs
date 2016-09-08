using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcApplication.Models;
using NUnit.Framework;

namespace Blog.UnitTests
{
    [TestFixture]
    public class PagingModelTests
    {
        [Test]
        [TestCaseSource("Pages")]
        public void Pages_should_return_limited_pages(int currentPage, int totalItemsCount, int itemsPerPage, List<int> pages)
        {
            Assert.AreEqual(pages, new PagingModel(currentPage, totalItemsCount, itemsPerPage).Pages.ToList());
        }

        private static IEnumerable<object[]> Pages()
        {
            yield return new object[] { 1, 1, 1, new List<int> { 1 } };
            yield return new object[] { 1, 25, 5, new List<int> { 1, 2, 3, 5 } };
            yield return new object[] { 3, 25, 5, new List<int> { 1, 2, 3, 4, 5 } };
            yield return new object[] { 4, 25, 5, new List<int> { 1, 2, 3, 4, 5 } };
            yield return new object[] { 5, 25, 5, new List<int> { 1, 3, 4, 5 } };
            yield return new object[] { 5, 50, 5, new List<int> { 1, 3, 4, 5, 6, 7, 10 } };
            yield return new object[] { 3, 50, 5, new List<int> { 1, 2, 3, 4, 5, 10 } };
        }
    }
}
