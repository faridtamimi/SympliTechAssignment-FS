using System.Collections.Generic;

namespace SeoEvaluation.Entities
{
    public class SearchInput
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
        public int NumberOfResults { get; set; }
        public List<string> SearchEngines { get; set; }
    }
}
