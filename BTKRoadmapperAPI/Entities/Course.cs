
namespace BTKRoadmapperAPI.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string CourseName { get; set; }
        public Category Category { get; set; } 
        public int TotalRequeiredTimeInSeconds { get; set; }
        public LevelInfo Level { get; set; } 
        public required string Description { get; set; }
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }

    public enum LevelInfo
    {
        Beginner,
        Intermediate,
        Advance
    }
    public enum Category
    {
        YazilimDunyasi = 0,
        SistemDunyasi = 1,
        IsletmeDunyasi = 2,
        KisiselGelisimDunyasi = 3,
        K12Dunyasi = 4,
        TasarimDunyasi = 5,
        KariyerYolu = 6,
        GuvenliInternet = 7,
        RegulasyonDunyasi = 8,
        YapayZekaDunyasi = 9,
        KurumVeKurulus = 10,
        TemelBilimler = 11
    }

}
