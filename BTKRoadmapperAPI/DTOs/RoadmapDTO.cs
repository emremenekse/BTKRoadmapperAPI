namespace BTKRoadmapperAPI.DTOs
{
    public class RoadmapDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string InterestedFields { get; set; } 
        public EducationLevel EducationLevel { get; set; } 
        public SkillLevel InterestedFieldSkillLevel { get; set; } 
        public int DailyAvailableTime { get; set; } 
        public TargetField TargetField { get; set; }
        public bool IsUser { get; set; }
        public bool HasUserInfoChange { get; set; }

    }

    public enum EducationLevel
    {
        HighSchool = 1,
        AssociateDegree = 2,
        Bachelor = 3,
        Master = 4,
        Doctorate = 5
    }

    public enum SkillLevel
    {
        NoExperience = 1,
        Beginner = 2,
        Intermediate = 3,
        Advanced = 4
    }

    public enum TargetField
    {
        WebDevelopment = 1,
        MobileDevelopment = 2,
        GameDevelopment = 3,
        DataScience = 4,
        ArtificialIntelligence = 5,
        Cybersecurity = 6
    }
}
