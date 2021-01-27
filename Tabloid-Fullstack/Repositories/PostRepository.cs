﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tabloid_Fullstack.Data;
using Tabloid_Fullstack.Models;
using Tabloid_Fullstack.Models.ViewModels;

namespace Tabloid_Fullstack.Repositories
{
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PostSummary> Get()
        {
            var regex = new Regex(@"\w*\s*");
            return _context.Post
                .Include(p => p.Category)
                .Where(p => p.IsApproved)
                .Where(p => p.PublishDateTime <= DateTime.Now)
                .OrderByDescending(p => p.PublishDateTime)
                .Select(p => new PostSummary()
                {
                    Id = p.Id,
                    ImageLocation = p.ImageLocation,
                    Title = p.Title,
                    AuthorId = p.UserProfileId,
                    AuthorName = p.UserProfile.DisplayName,
                    AbbreviatedText = p.Content.Substring(0, 200),
                    PublishDateTime = p.PublishDateTime,
                    Category = p.Category,
                    Content = p.Content,
                    wordCount = regex.Matches(p.Content).Count
                })
                .ToList();
        }

        public Post GetById(int id)
        {
            return _context.Post
                .Include(p => p.UserProfile)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.UserProfile)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public List<ReactionCount> GetReactionCounts(int postId)
        {
            return _context.Reaction
                .Select(r => new ReactionCount()
                {
                    Reaction = r,
                    Count = r.PostReactions.Count(pr => pr.PostId == postId)
                })
                .ToList();
        }





        public void Add(Comment comment)
        {
            comment.Active = true;
            _context.Add(comment);
            _context.SaveChanges();
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            return _context.UserProfile
                .Include(up => up.UserType)
                .Include(up => up.Post)
                .FirstOrDefault(up => up.FirebaseUserId == firebaseUserId);
        }
        public void Add(Post post)
        {
            _context.Add(post);
            _context.SaveChanges();
        }
    }
}
