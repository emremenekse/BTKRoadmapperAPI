namespace BTKRoadmapperAPI.DTOs
{
    public class UserPreferencesDTO
    {
        public int AvailableHoursPerWeek { get; set; }
        public required string Goal { get; set; }
        public required string Interest { get; set; }
    }
}
