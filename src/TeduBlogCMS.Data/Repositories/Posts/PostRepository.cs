using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeduBlogCMS.Core.Domain.Content;
using TeduBlogCMS.Core.Models.PageResult;
using TeduBlogCMS.Core.Models.PostDto;
using TeduBlogCMS.Core.Repositories.Posts;
using TeduBlogCMS.Core.SeedWorks;
using TeduBlogCMS.Data.SeedWorks;

namespace TeduBlogCMS.Data.Repositories.Posts
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly IMapper _mapper;

        public PostRepository(TeduBlogConText context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public Task<List<Post>> GetPopularPostsAsync(int count)
        {
            return _context.Posts.OrderByDescending(x => x.ViewCount).Take(count).ToListAsync();
        }

        public async Task<PageResult<PostInListDto>> GetPostPagingAsync(
            string keyword,
            Guid? categoryId,
            int pageIndex = 1,
            int pageSize = 10
        )
        {
            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            var totalRow = await query.CountAsync();

            query = query
                .OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PageResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize,
            };
        }
    }
}
