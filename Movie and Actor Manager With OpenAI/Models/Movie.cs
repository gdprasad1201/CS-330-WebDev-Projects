namespace Fall2024_Assignment3_gdprasad.Models {
    public class Movie {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Year { get; set; }
        public string? Genre { get; set; }
        public string? ImdbLink { get; set; }
        public string? PosterLink { get; set; } // URL to the movie's poster

        public List<MovieActor>? MovieActors { get; set; }
    }
}