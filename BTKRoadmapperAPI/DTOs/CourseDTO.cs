﻿using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public required string CourseName { get; set; }
        public Category Category { get; set; }
        public int RecommendedOrder { get; set; }
        public int TotalRequeiredTimeInSeconds { get; set; }
        public LevelInfo Level { get; set; }
        public string Description { get; set; }
        public List<ModuleDTO> Modules { get; set; }
    }
}
