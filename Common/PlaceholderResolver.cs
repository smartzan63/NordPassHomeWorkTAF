using System;
using System.Collections.Generic;
using System.Linq;

namespace NordPassHomeWorkTAF.Common
{
    public static class PlaceholderResolver
    {
        /// <summary>
        /// Resolves placeholders in a dictionary of endpoints with the corresponding values from a dictionary of replacements.
        /// </summary>
        /// <param name="endpoints">A dictionary of endpoints where the keys are the endpoint names and the values are the endpoint URLs with placeholders.</param>
        /// <param name="replacements">A dictionary of replacements where the keys are the placeholder names and the values are the replacement values.</param>
        /// <returns>A new dictionary of endpoints where the placeholders in the endpoint URLs have been replaced with the corresponding values from the replacements dictionary.</returns>
        /// <remarks>
        /// This method performs multiple passes over the endpoints dictionary until no more replacements can be made. This allows for replacements that contain other placeholders to be resolved correctly.
        /// </remarks>
        public static Dictionary<string, string> ResolvePlaceholders(Dictionary<string, string> endpoints, Dictionary<string, string> replacements)
        {
            var replacedEndpoints = new Dictionary<string, string>(endpoints);

            bool replacementsMade;
            do
            {
                replacementsMade = false;
                foreach (var endpoint in replacedEndpoints.ToList())
                {
                    var replacedValue = endpoint.Value;

                    foreach (var replacement in replacements)
                    {
                        var newValue = replacedValue.Replace($"{{{replacement.Key}}}", replacement.Value);
                        if (newValue != replacedValue)
                        {
                            replacedValue = newValue;
                            replacementsMade = true;
                        }
                    }

                    replacedEndpoints[endpoint.Key] = replacedValue;
                }
            } while (replacementsMade);

            return replacedEndpoints;
        }
    }

}
