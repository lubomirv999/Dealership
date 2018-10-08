namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;

    public class CommentService : ICommentService
    {
        private readonly DealershipDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(DealershipDbContext db, UserManager<ApplicationUser> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }

        public void Add(int carId, string content, string id, int? parentCommentId)
        {
            var author = this._userManager.FindByIdAsync(id).Result;

            Comment comment = new Comment();
            comment.CarId = carId;
            comment.Content = content;
            comment.UserId = author.Id;
            comment.ParentComment = FindById(parentCommentId);

            this._db.Comments.Add(comment);
            this._db.SaveChanges();
        }

        public Comment FindById(int? commentId)
        {
            if(commentId == null)
            {
                return null;
            }
            return this._db.Comments.FirstOrDefault(c => c.Id == commentId);
        }
    }
}
