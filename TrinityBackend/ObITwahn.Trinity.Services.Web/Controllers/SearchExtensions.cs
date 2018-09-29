using System;
using System.Linq;

namespace ObITwahn.Trinity.Services.Web.Controllers
{
    public static class SearchExtensions
        {
            public static int CountOccurrencies(this string value, string topic)
            {
                return !String.IsNullOrWhiteSpace(value)
                    ? value.Split(' ').Count(x => x.Contains(topic, StringComparison.InvariantCultureIgnoreCase))
                    : 0;

            }
        }
}