﻿using byalexblog.DAL;
using System.Collections.Generic;

namespace byalexblog.Models
{
    public class ArticleListModel
    {
        private readonly IEnumerable<IArticle> items;
        private readonly PagingModel paging;

        public ArticleListModel(PagingModel paging, IEnumerable<IArticle> items)
        {
            this.paging = paging;
            this.items = items;
        }

        public PagingModel Paging { get { return paging; } }
        public IEnumerable<IArticle> Items { get { return items; } }
    }
}