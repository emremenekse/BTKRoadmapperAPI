namespace BTKRoadmapperAPI.DTOs
{
    public class UserDTO
    {
        public  string Name { get; set; }
        public  string Email { get; set; }
        public  string Role { get; set; }
        public int AvailableHoursPerDaily { get; set; }
        public  string InterestedFields { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public SkillLevel InterestedFieldSkillLevel { get; set; }
        public TargetField TargetField { get; set; }
    }
}
