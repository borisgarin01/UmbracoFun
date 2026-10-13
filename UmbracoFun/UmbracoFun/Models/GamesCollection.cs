using System.ComponentModel.DataAnnotations;

namespace UmbracoFun.Models
{
    public class GamesCollection
    {

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageSource { get; set; }

        public List<GamesCollectionItem> GamesCollectionItems { get; set; } = new();
    }
}