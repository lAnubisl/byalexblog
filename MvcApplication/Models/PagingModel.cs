using System;

namespace MvcApplication.Models
{
    public class PagingModel
    {
        private readonly int currentPage, totalItemsCount;

        public PagingModel(int CurrentPage, int TotalItemsCount)
        {
            currentPage = CurrentPage;
            totalItemsCount = TotalItemsCount;
        }

        public int TotalPages { get { return (int)Math.Ceiling(TotalItemsCount / (double)ItemsPerPage); } }
        public int TotalItemsCount { get { return totalItemsCount; } }
        public int CurrentPage { get { return currentPage; } }
        public int ItemsPerPage { get { return 3; } }
    }
}