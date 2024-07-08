using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Constants
{
    public class SearchConstants
    {
        public static readonly Dictionary<string, string> Tokens = new()
        {
            { "a level", "advanced gce" },
            { "a levels", "advanced gce" },
            { "a-level", "advanced gce" },
            { "a-levels", "advanced gce" },
            { "as level", "advanced subsidiary gce" },
            { "as levels", "advanced subsidiary gce" },
            { "as-level", "advanced subsidiary gce" },
            { "as-levels", "advanced subsidiary gce" },
            { "aea", "advanced extension award" },
            { "t level", "t level" },
            { "t levels", "t level" },
            { "t-level", "t level" },
            { "t-levels", "t level" },
        };

        public static readonly List<string> StopWords =
        [
            "a",
            "about",
            "above",
            "after",
            "again",
            "against",
            "all",
            "am",
            "an",
            "and",
            "any",
            "are",
            "as",
            "at",
            "be",
            "because",
            "been",
            "before",
            "being",
            "below",
            "between",
            "both",
            "but",
            "by",
            "can",
            "did",
            "do",
            "does",
            "doing",
            "don",
            "down",
            "during",
            "each",
            "few",
            "for",
            "from",
            "further",
            "had",
            "has",
            "have",
            "having",
            "he",
            "her",
            "here",
            "hers",
            "herself",
            "him",
            "himself",
            "his",
            "how",
            "i",
            "if",
            "in",
            "into",
            "is",
            "it",
            "its",
            "itself",
            "just",
            "me",
            "more",
            "most",
            "my",
            "myself",
            "no",
            "nor",
            "not",
            "now",
            "of",
            "off",
            "on",
            "once",
            "only",
            "or",
            "other",
            "our",
            "ours",
            "ourselves",
            "out",
            "over",
            "own",
            "s",
            "same",
            "she",
            "should",
            "so",
            "some",
            "such",
            "t",
            "than",
            "that",
            "the",
            "their",
            "theirs",
            "them",
            "themselves",
            "then",
            "there",
            "these",
            "they",
            "this",
            "those",
            "through",
            "to",
            "too",
            "under",
            "until",
            "up",
            "very",
            "was",
            "we",
            "were",
            "what",
            "when",
            "where",
            "which",
            "while",
            "who",
            "whom",
            "why",
            "will",
            "with",
            "you",
            "your",
            "yours",
            "yourself",
            "yourselves",
            "\"",
            "\'",
        ];

        public static readonly List<string> SplitLimiters =
        [
            ",",
            ".",
            ";",
            ":",
            "–",
            "/",
            @"\",
            "|",
            "+",
            "_",
            "!",
            "?",
            "(",
            ")",
            "[",
            "]",
            "{",
            "}"
        ];
    }
}
