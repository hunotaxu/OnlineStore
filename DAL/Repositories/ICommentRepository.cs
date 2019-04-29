using DAL.Repositories.Base;
using DAL.Models;
using System.Collections.Generic;
using DAL.Data.Entities;


namespace DAL.Repositories
{
    public interface ICommentRepository : IRepo<Comment>
    {
        IEnumerable<Comment> GetByItemId(int itemId);
        Comment GetComment(int? id);

    }
}
