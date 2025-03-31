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
    /// Represents a single entry in an M3U file.
    /// </summary>
    public class M3UEntry
    {
        /// <summary>
        /// Gets or sets the title of the entry.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL of the entry.
        /// </summary>
        public required string Url { get; set; }
    }
}
