using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace ShopMobile.Models
{
    public class Pager
    {
        public Pager()
        {
        }



        public Pager(int curentPage, int totalPage, int limit)
        {
            this.curentPage = curentPage;
            this.totalPage = totalPage;
            this.limit = limit;
            int startPage = curentPage - 5;
            int endPage = curentPage  + 4;
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPage)
            {
                endPage = totalPage;
                if (endPage > 10)
                {
                    startPage = startPage - 9;
                }
            }
            this.startPage = startPage;
            this.endPage = endPage;
        }

        public int curentPage { get; set; }
        public int totalPage { get; set; }
        public int limit { get; set; }
        public int startPage { set; get; }
        public int endPage { set; get; }
    }



}
