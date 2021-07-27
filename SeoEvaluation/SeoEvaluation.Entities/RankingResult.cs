using System.Collections.Generic;

namespace SeoEvaluation.Entities
{
    public class RankingResult
    {
        public string SearchEngineName { get; set; }

        public List<int> Rankings { get; set; }
    }
}
