using AutoMapper;
using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Data;
using EcommerceApp.Enums;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public BlogService(ApplicationDbContext context, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<List<BlogPostDto>> GetPublishedPostsAsync(string? searchTerm = null, BlogCategory? category = null)
        {
            var query = _context.BlogPosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Where(p => p.IsPublished);

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(searchTerm) || 
                                       (p.Content != null && p.Content.ToLower().Contains(searchTerm)) ||
                                       (p.Tags != null && p.Tags.ToLower().Contains(searchTerm)));
            }

            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<BlogPostDto>>(posts);
        }

        public async Task<BlogPostDto?> GetPostDetailsAsync(int id, int? currentUserId = null)
        {
            var post = await _context.BlogPosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments!)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (post == null) return null;

            var dto = _mapper.Map<BlogPostDto>(post);
            if (currentUserId.HasValue && post.Likes != null)
            {
                dto.IsLikedByCurrentUser = post.Likes.Any(l => l.UserId == currentUserId.Value);
            }
            
            dto.Comments = dto.Comments.OrderByDescending(c => c.CreatedAt).ToList();
            return dto;
        }

        public async Task<bool> ToggleLikeAsync(int postId, int userId)
        {
            var like = await _context.Likes.FindAsync(userId, postId);
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                return false; // Result is NOT liked
            }
            else
            {
                _context.Likes.Add(new Like { UserId = userId, BlogPostId = postId });
                await _context.SaveChangesAsync();

                // Notification
                var user = await _context.Users.FindAsync(userId);
                var post = await _context.BlogPosts.FindAsync(postId);
                await _notificationService.CreateNotificationAsync(
                    "Lượt thích mới",
                    $"{user?.FullName ?? user?.UserName} đã thích bài viết: {post?.Title}",
                    Enums.NotificationType.NewLike,
                    $"/Blog/Details/{postId}"
                );

                return true; // Result is LIKED
            }
        }

        public async Task<CommentDto> AddCommentAsync(int postId, int userId, string content)
        {
            var comment = new Comment
            {
                BlogPostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Notification
            var post = await _context.BlogPosts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);
            await _notificationService.CreateNotificationAsync(
                "Bình luận mới",
                $"{user?.FullName ?? user?.UserName} đã bình luận bài viết: {post?.Title}",
                Enums.NotificationType.NewComment,
                $"/Blog/Details/{postId}"
            );

            // Reload to get User info
            var savedComment = await _context.Comments
                .Include(c => c.User)
                .FirstAsync(c => c.ID == comment.ID);

            return _mapper.Map<CommentDto>(savedComment);
        }

        public async Task<List<BlogPostDto>> GetAllPostsAsync()
        {
            var posts = await _context.BlogPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<BlogPostDto>>(posts);
        }

        public async Task<BlogPostDto> CreatePostAsync(int userId, CreateBlogPostDto dto)
        {
            var post = _mapper.Map<BlogPost>(dto);
            post.UserId = userId;
            post.CreatedAt = DateTime.Now;

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogPostDto>(post);
        }

        public async Task<bool> UpdatePostAsync(int id, CreateBlogPostDto dto)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null) return false;

            _mapper.Map(dto, post);
            post.ModifiedAt = DateTime.Now;

            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null) return false;

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddPostImagesAsync(int postId, List<string> imageUrls)
        {
            var post = await _context.BlogPosts.Include(p => p.Images).FirstOrDefaultAsync(p => p.ID == postId);
            if (post == null) return false;

            int nextOrder = (post.Images?.Any() == true) ? post.Images.Max(i => i.SortOrder) + 1 : 1;

            foreach (var url in imageUrls)
            {
                _context.BlogPostImages.Add(new BlogPostImage
                {
                    BlogPostId = postId,
                    ImageUrl = url,
                    SortOrder = nextOrder++
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
