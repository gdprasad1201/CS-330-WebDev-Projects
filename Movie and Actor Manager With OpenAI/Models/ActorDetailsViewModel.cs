namespace Fall2024_Assignment3_gdprasad.Models {
    public class ActorDetailsViewModel {
        public Actor Actor { get; set; }
        public List<Movie> Movies { get; set; }

        // A Table containing two columns: Twenty tweets generated from the "Twitter" API generated by AI relating to the selected actor and the analysis of the sentiment of the tweets.

        public List<(string, double)> TweetsANDSentiments { get; set; } = new List<(string, double)>();
        public double overallSentiment { get; set; }

        public ActorDetailsViewModel(Actor actor, List<Movie> movies, List<(string, double)> tweetsANDSentiments, double overallSentiment) {
            Actor = actor;
            Movies = movies;
            TweetsANDSentiments = tweetsANDSentiments;
            this.overallSentiment = overallSentiment;
        }
    }
}