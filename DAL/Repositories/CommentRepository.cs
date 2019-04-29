using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using System;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class CommentRepository : RepoBase<Comment>, ICommentRepository
    {
        public CommentRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
        public IEnumerable<Comment> GetByItemId(int itemId)
        {
            return GetSome(y => y.ItemId == itemId && y.IsDeleted == false, y => y.Content, true).ToList();
        }
        public Item GetComment(int? id)
        {
            throw new NotImplementedException();
        }
        Comment ICommentRepository.GetComment(int? id)
        {
            throw new NotImplementedException();
        }
    }
}