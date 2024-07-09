using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DataBase_practice_Day_33_
{

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public string DbPath { get; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }
        // The following configures EF to create a Sqlite database file in the special "Local" folder from your platform
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; } = new();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }



    internal class Program
    {
        static void Main(string[] args)
        {   //This is common CRUD
            using var db = new BloggingContext();

            //Note this sample requires the database to be created before running.
            Console.WriteLine($"Database path: {db.DbPath}.");

            //This is creating
            Console.WriteLine("Inserting a new blog");
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.SaveChanges();

            //This is reading
            Console.WriteLine("Querying for a blog");
            var blog = db.Blogs.OrderBy(blog => blog.BlogId).First();

            //This us updating
            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote and app using EF Core!" });
            db.SaveChanges();

            //This is deleting the application
            Console.WriteLine("Delete the blog");
            db.Remove(blog);
            db.SaveChanges();
        }
    }
}
