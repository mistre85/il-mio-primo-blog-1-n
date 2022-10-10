using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Data;
using NetCore.Models;
using NetCore_01.Models;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Mime;

namespace NetCore.Controllers
{
    public class PostController : Controller
    {
        public PostController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (BlogContext context = new BlogContext())
            {
                List<Post> posts = context.Posts.Include("Category").Include("Tags").ToList() ;

                return View("Index", posts);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                //Post postFound = context.Posts.Where(post => post.Id == id).Include("Category").FirstOrDefault();
                Post postFound = context.Posts.Where(post => post.Id == id)
                    .Include(post => post.Category).Include(post => post.Tags).FirstOrDefault();
                    
                if (postFound == null)
                {
                    return NotFound($"Il post con id {id} non è stato trovato");
                }
                else
                {
                    return View("Details", postFound);
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            PostsCategories postsCategories = new PostsCategories();

            //postsCategories.Post è già a posto? no ...
            //postsCategories.Post = new Post(); ...  dobbiamo inizializzarlo se non lo fa il costruttore
            BlogContext context = new BlogContext();

            postsCategories.Categories = context.Categories.ToList();

            postsCategories.Tags = context.Tags.ToList();

            return View(postsCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostsCategories formData)
        {
            BlogContext context = new BlogContext();

            if (!ModelState.IsValid)
            {
                formData.Categories = context.Categories.ToList();
                formData.Tags = context.Tags.ToList();
                return View("Create", formData);
            }

            //recupero tag reali dal db
            //List<Tag> selectedTags = new List<Tag>();

            //formData.Post.Tags = new List<Tag>();
            //foreach(int tagId in formData.SelectedTags)
            //{
            //    Tag tag = context.Tags.Where(tag => tag.Id == tagId).FirstOrDefault();
            //    //todo: check null
            //    //selectedTags.Add(tag);
            //    formData.Post.Tags.Add(tag);
            //}
            //fine recupero tag

            formData.Post.Tags = context.Tags.Where(tag => formData.SelectedTags.Contains(tag.Id)).ToList<Tag>();

            //assegno al POST che l'utente ha creato
            //i tag che ha selezionato
           // formData.Post.Tags = selectedTags;

            context.Posts.Add(formData.Post);

            context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                Post postToEdit = context.Posts.Include("Category").Include("Tags").
                    Where(post => post.Id == id).FirstOrDefault();

                if (postToEdit == null)
                {
                    return NotFound();
                }
                
                PostsCategories postsCategories = new PostsCategories();

                postsCategories.Post = postToEdit; //model del db
                postsCategories.Categories = context.Categories.ToList(); //<select che serve alla vista
                postsCategories.Tags = context.Tags.ToList();

                return View(postsCategories);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PostsCategories formData)
        {
            using (BlogContext context = new BlogContext())
            {
                if (!ModelState.IsValid)
                {
                    formData.Categories = context.Categories.ToList();
                    formData.Tags = context.Tags.ToList();
                    return View("Update", formData);
                }

                Post post = context.Posts.Where(post => post.Id == id).Include("Tags").FirstOrDefault();

                post.Title = formData.Post.Title;
                post.Description = formData.Post.Description;
                post.CategoryId = formData.Post.CategoryId;
                post.Tags = context.Tags.Where(tag => formData.SelectedTags.Contains(tag.Id)).ToList<Tag>();
             
               
                context.SaveChanges();

                return RedirectToAction("Index");
               
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                Post postToDelete = context.Posts.Where(post => post.Id == id).FirstOrDefault();

                if (postToDelete != null)
                {
                    context.Posts.Remove(postToDelete);

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}