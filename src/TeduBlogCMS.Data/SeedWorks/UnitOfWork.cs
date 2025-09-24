using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduBlogCMS.Core.SeedWorks;

namespace TeduBlogCMS.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TeduBlogConText _context;

        public UnitOfWork(TeduBlogConText context) => _context = context;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}