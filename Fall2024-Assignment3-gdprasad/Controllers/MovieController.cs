using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_gdprasad.Data;
using Fall2024_Assignment3_gdprasad.Models;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using VaderSharp2;
using System.ClientModel;

namespace Fall2024_Assignment3_gdprasad.Controllers {
    public class MovieController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly string ApiKey;
        private const string ApiEndpoint = "https://fall2024-gdprasad-openai.openai.azure.com/";
        private const string AiDeployment = "gpt-35-turbo";
        private readonly ApiKeyCredential ApiCredential;

        public MovieController(ApplicationDbContext context, IConfiguration configuration) {
            _context = context;
            ApiKey = configuration["OpenAI:ServiceApiKey"] ?? throw new ArgumentNullException("OpenAI:ServiceApiKey", "API key not found in configuration.");
            ApiCredential = new(ApiKey);
            _context = context;
        } // GET: Movie

        public async Task<List<(string, double)>> GetMovieReviews(string MovieName) {
            Console.WriteLine("Asking Reviewers...");
            var reviewWithSentiment = new List<(string, double)>();
            ChatClient chatClient = new AzureOpenAIClient(new Uri(ApiEndpoint), ApiCredential).GetChatClient(AiDeployment);

            string[] personalities = ["The Critic", "The Fan", "The Cinephile", "The Skeptic", "The Optimist", "The Pessimist", "The Realist", "The Idealist", "The Cynic", "The Romantic"];
            var movieReviews = new List<string>();
            foreach (string persona in personalities) {
                var messages = new ChatMessage[] {
                    new SystemChatMessage($"Do not say you are an AI langauge model. You must take on the role of a film reviewer who embodies {persona}. You are reviewing the movie {MovieName}."),
                    new UserChatMessage($"How would you rate the movie {MovieName} out of 10 in less than 20 words?")
                };
                var chatCompletionOptions = new ChatCompletionOptions {
                    MaxOutputTokenCount = 200,
                };
                ClientResult<ChatCompletion> result = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);

                movieReviews.Add(result.Value.Content[0].Text);
                Thread.Sleep(TimeSpan.FromSeconds(5)); 
            }

            //calculate the sentiment for each review
            var analyzer = new SentimentIntensityAnalyzer();
            foreach (var review in movieReviews) {
                reviewWithSentiment.Add((review, analyzer.PolarityScores(review).Compound));
            }

            return reviewWithSentiment;
        }

        public async Task<IActionResult> Index() {
            return View(await _context.Movies.ToListAsync());
        } // GET: Movie/Details/5

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            
            var movie = await _context.Movies
                .Include(ma => ma.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            var movieReviewsSentiments = new List<(string, double)>();
            if (movie != null && !string.IsNullOrEmpty(movie.Name)) {
                movieReviewsSentiments = await GetMovieReviews(movie.Name);
            }
            var averageSentiment = movieReviewsSentiments.Select(r => r.Item2).DefaultIfEmpty(0).Average();
            
            if (movie == null) {
                return NotFound();
            }

            var viewMovieModel = new MovieDetailsViewModel(movie, movie.MovieActors.Select(ma => ma.Actor).ToList(), movieReviewsSentiments, averageSentiment);

            return View(viewMovieModel);
        }

        // GET: Movie/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Year,Genre,ImdbLink,PosterLink")] Movie movie) {
            if (ModelState.IsValid) {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Year,Genre,ImdbLink,PosterLink")] Movie movie) {
            if (id != movie.Id) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                try {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!MovieExists(movie.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) {
                return NotFound();
            }
            return View(movie);
        } 
        
        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null) {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id) {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}