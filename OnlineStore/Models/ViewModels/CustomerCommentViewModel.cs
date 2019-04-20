using System;

namespace OnlineStore.Models.ViewModels
{
    public class CustomerCommentViewModel
    {
        public string CustomerName;
        public Guid CustomerId;
        public int CommentId;
        public string Content;
        public int ItemId;
        public int Evaluation;
        public DateTime Time;
    }
}
