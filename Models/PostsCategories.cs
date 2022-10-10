using NetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace NetCore_01.Models
{
    //non è un model del DB
    public class PostsCategories
    {
        //action [view,db] create - questo è un model del db
        public Post Post { get; set; } 

        //action [view] è proprio la <select .. option...
        public List<Category> Categories { get; set; }

        //action [view] è proprio la <select .. option...
        public List<Tag> Tags { get; set; }

        //action [post] la lista dei tag che vengono selezionati dall'utente
        public List<int> SelectedTags { get; set; }

        public PostsCategories()
        {
            Post = new Post();
            Categories = new List<Category>();
            Tags = new List<Tag>();
            SelectedTags = new List<int>();
        }
    }
}
