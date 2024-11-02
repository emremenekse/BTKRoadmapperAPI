using BTKRoadmapperAPI.DTOs;

namespace BTKRoadmapperAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public int AvailableHoursPerDaily { get; set; }
        public required string InterestedFields { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public SkillLevel InterestedFieldSkillLevel { get; set; }
        public TargetField TargetField { get; set; }

    }

}
