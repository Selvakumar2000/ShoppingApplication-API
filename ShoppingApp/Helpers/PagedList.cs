using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Helpers
{
    //IEnumerable is an interface. List is a class that defines a generic collection
    //List is also implements from IEnumerable interface.
    //When you run a LINQ it returns an IEnumerable<T>.
    //Ref --> https://medium.com/@ben.k.muller/c-ienumerable-vs-list-and-array-9f099f157f4f
    //Ref --> https://www.tutorialspoint.com/What-is-the-AddRange-method-in-Chash-lists
    //Ref --> https://www.c-sharpcorner.com/forums/ienumerable-vs-lis

    public class PagedList<T> : List<T>  //T can be determined during runtime T may be any DTO
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static  PagedList<T> Create(IList<T> source, int pageNumber, int pageSize)
        {
            var count =  source.Count; 
            var items =  source.OfType<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
