namespace NetCore.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }


        public List<Post> Posts { get; set; }

        public Category()
        {
            //ef e validation
        }
    }
}