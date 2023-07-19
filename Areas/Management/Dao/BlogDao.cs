using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class BlogDao
    {
        private static BlogDao? instance = null;
        private BlogDao() { }

        internal static BlogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BlogDao();
                }
                return instance;
            }
        }


        public void Addevent(string Name, string Image, string Description)
        {
            Blog blog= new Blog();
            blog.Name = Name;
            blog.Image = Image;
            blog.Description = Description;
            NexusContext nexusContext = new NexusContext();
            nexusContext.Add(blog);
            nexusContext.SaveChanges();
        }
        public List<Blog> Blogs {
            get

            {
                NexusContext f = new NexusContext();
                var result = f.Blogs.Select(x => new Blog() { Id = x.Id, Name = x.Name, Image = x.Image, Description = x.Description}).OrderByDescending(x=>x.Id).ToList();
                return result;
            }

        }
        public Blog blog1(int id)
        {
            NexusContext f = new NexusContext();
            Blog b= f.Blogs.Find(id);
            return b;
        }
        public Blog EditBlog(int Id)
        {
            NexusContext f = new NexusContext();
            Blog eblog = f.Blogs.Find(Id);
            return eblog;
        }
    }

}
