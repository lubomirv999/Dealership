namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models;
    using Microsoft.AspNetCore.Identity;

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
            comment.ParentCommentId = parentCommentId;

            this._db.Comments.Add(comment);
            this._db.SaveChanges();
        }
    }
}
