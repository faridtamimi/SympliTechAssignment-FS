using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeoEvaluation.Api.DTOs
{
    public record SearchInputDto
    {
        [Required]
        public string Keywords { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int NumberOfResults { get; set; }
        
        public List<string> SearchEngines { get; set; }
    }
}
