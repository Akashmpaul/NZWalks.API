using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "code has to be a minimum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage ="Nmae has to be a maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
