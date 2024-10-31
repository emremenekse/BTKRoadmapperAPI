using System.Text.Json.Serialization;

namespace BTKRoadmapperAPI.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public required string Title { get; set; } 
        public int LessonCount { get; set; } 
        public int CourseId { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
    }

}
