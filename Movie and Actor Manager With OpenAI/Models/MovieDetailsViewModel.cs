using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace Fall2024_Assignment3_gdprasad.Models {
    public class MovieDetailsViewModel {
        public Movie Movie { get; set; }
        public List<Actor> Actors { get; set; }
        public List<(string, double)> ReviewsANDSentiments { get; set; } = new List<(string, double)>();
        public double overallSentiment { get; set; }

        public MovieDetailsViewModel(Movie movie, List<Actor> actors, List<(string, double)> reviewsANDSentiments, double overallSentiment) {
            Movie = movie;
            Actors = actors;
            ReviewsANDSentiments = reviewsANDSentiments;
            this.overallSentiment = overallSentiment;
        }
    }
}