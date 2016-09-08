using System;
using System.Collections.Generic;

namespace MvcApplication.Models
{
    public class PagingModel
    {
        private readonly int currentPage, totalPages;

        public PagingModel(int currentPage, int totalItemsCount, int itemsPerPage)
        {
            this.currentPage = currentPage;
            this.totalPages = (int) Math.Ceiling(totalItemsCount/(double) itemsPerPage);
        }

        public IEnumerable<int> Pages
        {
            get
            {
                if (ShowHellipAfter(1))
                {
                    yield return 1;
                }
                for (var i = Math.Max(1, currentPage - 2); i <= Math.Min(totalPages, currentPage + 2); i++)
                {
                    yield return i;
                }
                if (ShowHellipBefore(totalPages))
                {
                    yield return totalPages;
                }
            }
        }

        public bool ShowHellipBefore(int pageNumber)
        {
            return pageNumber == totalPages && totalPages - currentPage > 3;
        }

        public bool ShowHellipAfter(int pageNumber)
        {
            return pageNumber == 1 && currentPage > 3;
        }

        public bool IsCurrent(int pageNumber)
        {
            return pageNumber == currentPage;
        }

        public int NextPage { get { return currentPage + 1; } }

        public int PreviousPage { get { return currentPage - 1; } }

        public bool ShowButtonPrevious { get { return currentPage > 1; } }

        public bool ShowButtonNext { get { return currentPage < totalPages; } }
    }
}