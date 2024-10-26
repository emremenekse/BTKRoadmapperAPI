
namespace BTKRoadmapperAPI.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string CourseName { get; set; }
        public required string Category { get; set; } 
        public required string Level { get; set; } 
        public required string Description { get; set; }
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }

}
