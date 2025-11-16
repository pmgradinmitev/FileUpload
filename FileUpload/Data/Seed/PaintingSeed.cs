using Microsoft.EntityFrameworkCore;
using FileUpload.Data.Entities;

namespace FileUpload.Data.Seed
{
    public class PaintingSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            //Looks for any paintings. If no found, seeds the table.
            if (!context.Paintings.Any())
            {
                var paintings = new[]
                 {
                    new Painting { Name = "Starry Night", Author = "Vincent van Gogh", Description = "A depiction of van Gogh's view from the asylum.", ImagePath = "files/painting-images/painting1.jpg" },
                    new Painting { Name = "Mona Lisa", Author = "Leonardo da Vinci", Description = "Famous portrait with enigmatic smile.", ImagePath = "files/painting-images/painting2.jpg" },
                    new Painting { Name = "The Scream", Author = "Edvard Munch", Description = "Expressionist depiction of anxiety.", ImagePath = "files/painting-images/painting3.jpg" },
                    new Painting { Name = "The Kiss", Author = "Gustav Klimt", Description = "Ornate painting of a couple in gold patterns.", ImagePath = "files/painting-images/painting4.jpg" }
                };

                // Copy image files to wwwroot
                var wwwRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var imagesFolder = Path.Combine(wwwRoot, "files", "painting-images");
                Directory.CreateDirectory(imagesFolder);

                // Assume sample images exist in a folder called "SeedImages" in the project root
                var sourceFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seed", "Images");

                for (int i = 0; i < paintings.Length; i++)
                {
                    var sourceFile = Path.Combine(sourceFolder, $"painting{i + 1}.jpg");
                    var destFile = Path.Combine(imagesFolder, $"painting{i + 1}.jpg");

                    if (File.Exists(sourceFile))
                    {
                        File.Copy(sourceFile, destFile, overwrite: true);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Source file not found: {sourceFile}");
                    }
                }
                context.Paintings.AddRange(paintings);
                context.SaveChanges();
            }
        }
    }
}
