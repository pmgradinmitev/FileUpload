using FileUpload.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FileUpload.ViewModels
{
    public class PaintingViewModel : IValidatableObject
    {
        public int? Id { get; set; }

        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Име")]
        public string Name {  get; set; }
        [StringLength(60, MinimumLength = 10)]
        [Display(Name = "Автор")]
        public string Author {  get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [ValidateNever]
        public string ImagePath {  get; set; }
        public IFormFile? File {  get; set; }

        public void MapTo(Painting entity)
        {
            entity.Name = Name;
            entity.Author = Author;
            entity.Description = Description;
            entity.ImagePath = ImagePath;
        }
        public void MapFrom(Painting entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Author = entity.Author;
            Description = entity.Description;
            ImagePath = entity.ImagePath;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var errorList = new List<ValidationResult>();
            if (Id == null && File == null)
            {
                errorList.Add(new ValidationResult(
                    "Полето е задължително!",
                    new[] { nameof(File) }
                ));
            }
            else if (File != null) // on update, only if we upload a file
            {
                var extension = Path.GetExtension(File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    errorList.Add(new ValidationResult(
                    $"Невалиден файлов формат! Допустими: {string.Join(", ", allowedExtensions)}",
                    new[] { nameof(File) }
                    ));
                }
            }
            return errorList;
        }
    }
}
