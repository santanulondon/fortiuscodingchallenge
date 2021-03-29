using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }

        /// <summary>
        /// Searches the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">options</exception>
        public SearchResults Search(SearchOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var shirtsSizesToBeSearch = options.Sizes.ToHashSet();
            var shirtsColorsToBeSearch = options.Colors.ToHashSet();

            var matchedShirts = _shirts.Where(shirt =>
                (shirtsSizesToBeSearch.Count == 0 || shirtsSizesToBeSearch.Contains(shirt.Size)) &&
                (shirtsColorsToBeSearch.Count == 0 || shirtsColorsToBeSearch.Contains(shirt.Color)));

            var shirtsSizeCounts = matchedShirts.GroupBy(s => s.Size).Select(p => new SizeCount { Size = p.Key, Count = p.Count() });
            var unmatchedShirtsSizeCounts = Size.All.Where(s => !shirtsSizeCounts.Any(sc => sc.Size == s)).Select(s => new SizeCount { Size = s, Count = 0 });

            var colorCounts = matchedShirts.GroupBy(s => s.Color).Select(p => new ColorCount { Color = p.Key, Count = p.Count() });
            var unmatchedColorCounts = Color.All.Where(c => !colorCounts.Any(cc => cc.Color == c)).Select(c => new ColorCount { Color = c });

            return new SearchResults
            {
                Shirts = matchedShirts.ToList(),
                SizeCounts = shirtsSizeCounts.Union(unmatchedShirtsSizeCounts).ToList(),
                ColorCounts = colorCounts.Union(unmatchedColorCounts).ToList(),
            };
        }
    }
}