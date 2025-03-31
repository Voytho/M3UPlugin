using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace M3UPlugin
{
    /// <summary>
    /// Helper class for processing M3U files.
    /// </summary>
    public static class M3UProcessor
    {
        /// <summary>
        /// Parses the M3U file and returns a collection of entries.
        /// </summary>
        /// <param name="filePath">The path to the M3U file.</param>
        /// <returns>A collection of M3UEntry instances.</returns>
        public static Collection<M3UEntry> ParseM3U(string filePath)
        {
            var entries = new Collection<M3UEntry>();
            if (!File.Exists(filePath))
            {
                return entries;
            }

            var lines = File.ReadAllLines(filePath);
            string? title = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("#EXTINF", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(line, "tvg-name=\"(.*?)\"");
                    title = match.Success ? match.Groups[1].Value : line.Split(',').Last().Trim();
                }
                else if (line.StartsWith("http", StringComparison.OrdinalIgnoreCase) && title != null)
                {
                    entries.Add(new M3UEntry { Title = title, Url = line.Trim() });
                    title = null;
                }
            }

            return entries;
        }
    }
}
