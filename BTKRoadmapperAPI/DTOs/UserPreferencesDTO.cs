namespace BTKRoadmapperAPI.DTOs
{
    public class UserPreferencesDTO
    {
        public int UserId { get; set; }
        public int AvailableHoursPerDaily { get; set; }
        public required string Interest { get; set; }
    }
}
