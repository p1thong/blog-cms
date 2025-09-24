using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TeduBlogCMS.Core.Models.PageResult
{
    public class PageResult<T> : PageResultBase
        where T : class
    {
        public List<T> Results { get; set; }

        public PageResult() => Results = new List<T>();
    }
}
