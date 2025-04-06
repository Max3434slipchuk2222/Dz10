using Bogus;

namespace WebAliona.Data
{
    public class DataBaseManager
    {
        public async Task AddNews(AppAlionaContext context)
        {
            var faker = new Faker<News>("uk")
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(5))
                .RuleFor(b => b.Slug, f => f.Lorem.Sentence(5))
                .RuleFor(b => b.Summary, f => f.Lorem.Sentence(10))
                .RuleFor(b => b.Image, f => f.Image.PicsumUrl(640, 480))
                .RuleFor(b => b.Content, f => f.Lorem.Text());

            for (int i = 0; i < 20; i++)
            {
                var b = faker.Generate(1);
                await context.AddAsync(b[0]);
                await context.SaveChangesAsync();
            }
        }
    }
}