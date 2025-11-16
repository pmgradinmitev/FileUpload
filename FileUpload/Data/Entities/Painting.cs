using System.ComponentModel.DataAnnotations;

namespace FileUpload.Data.Entities
{
    public class Painting
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(60)]
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImagePath {  get; set; }
    }
}
