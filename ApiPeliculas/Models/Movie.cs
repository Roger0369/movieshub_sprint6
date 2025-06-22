namespace ApiPeliculas.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Image { get; set; }

        public bool? IsFavorite { get; set; }
        public bool? IsHidden { get; set; }
    }
}