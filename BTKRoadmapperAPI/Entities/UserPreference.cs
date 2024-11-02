namespace BTKRoadmapperAPI.Entities
{
    public class UserPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int AvailableHoursPerDaily { get; set; } 
        public required string Interest{ get; set; }
        public virtual User User { get; set; }
    }

}
