namespace SeoEvaluation.Api.DTOs
{
    public record RankingResultDto
    {
        public string SearchEngineName { get; set; }

        public string Rankings { get; set; }
    }
}
