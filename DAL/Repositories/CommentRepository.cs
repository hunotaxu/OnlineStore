using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepository : RepoBase<Comment>, ICommentRepository
    {
        public CommentRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}