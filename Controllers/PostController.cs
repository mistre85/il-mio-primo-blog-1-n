using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Data;
using NetCore.Models;
using NetCore_01.Models;
using System.Diagnostics;
using System.Linq.Expressions;

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
                List<Post> posts = context.Posts.Include("Category").ToList();

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
                    .Include(post => post.Category).FirstOrDefault();
                    
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

            postsCategories.Categories = new BlogContext().Categories.ToList();

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
                return View("Create", formData);
            }


            //Post postToCreate = new Post();

            //postToCreate.Title = formData.Post.Title;
            //postToCreate.Description = formData.Post.Description;
            //postToCreate.Image = formData.Post.Image;
            //postToCreate.CategoryId = formData.Post.CategoryId;

            context.Posts.Add(formData.Post);

        
            context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (BlogContext context = new BlogContext())
            {
                Post postToEdit = context.Posts.Where(post => post.Id == id).FirstOrDefault();

                if (postToEdit == null)
                {
                    return NotFound();
                }
                
                PostsCategories postsCategories = new PostsCategories();

                postsCategories.Post = postToEdit; //model del db
                postsCategories.Categories = context.Categories.ToList(); //<select che serve alla vista

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
                    return View("Update", formData);
                }

                //Post postToEdit = context.Posts.Where(post => post.Id == id).FirstOrDefault();


                //aggiorniamo i campi con i nuovi valori
                //postToEdit.Title = formData.Post.Title;
                //postToEdit.Description = formData.Post.Description;
                //postToEdit.Image = formData.Post.Image;
                //postToEdit.CategoryId = formData.Post.CategoryId;

                formData.Post.Id = id;
                context.Posts.Update(formData.Post);

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