using TeduBlogCMS.Core.Domain.Content;
using TeduBlogCMS.Core.Models.PageResult;
using TeduBlogCMS.Core.Models.PostDto;
using TeduBlogCMS.Core.SeedWorks;

namespace TeduBlogCMS.Core.Repositories.Posts
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<List<Post>> GetPopularPostsAsync(int count);
        Task<PageResult<PostInListDto>> GetPostPagingAsync(
            string keyword,
            Guid? categoryId,
            int pageIndex = 1,
            int pageSize = 10
        );
    }
}
