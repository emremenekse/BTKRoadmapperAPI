namespace BTKRoadmapperAPI.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public required string Title { get; set; } 
        public int LessonCount { get; set; } 
        public required string Summary { get; set; } 
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }

}
