namespace Fall2024_Assignment3_gdprasad.Models {
    public class Actor {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public string? ImdbLink { get; set; }
        public string? PhotoLink { get; set; } // URL to the actor's photo 

        public List<MovieActor>? MovieActors { get; set; }
    }
}