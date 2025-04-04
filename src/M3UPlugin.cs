using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Jellyfin.Plugin.Template.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace M3UPlugin
{
    /// <summary>
    /// Klasa główna pluginu.
    /// </summary>
    public class M3UPlugin : BasePlugin
    {
        private readonly IApplicationPaths _applicationPaths;
        private readonly IXmlSerializer _xmlSerializer;
        private readonly ILibraryManager _libraryManager;
        private readonly ILogger<M3UPlugin> _logger;
        private readonly PluginConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="M3UPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Ścieżki aplikacji.</param>
        /// <param name="xmlSerializer">Instancja serializatora XML.</param>
        /// <param name="libraryManager">Zarządzanie biblioteką.</param>
        /// <param name="loggerFactory">Instancja fabryki loggerów.</param>
        public M3UPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILibraryManager libraryManager, ILoggerFactory loggerFactory)
        {
            _applicationPaths = applicationPaths;
            _xmlSerializer = xmlSerializer;
            _libraryManager = libraryManager;
            _logger = loggerFactory.CreateLogger<M3UPlugin>();
            _configuration = new PluginConfiguration();
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public override string Name => "M3UPlugin";

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public override string Description => "Plugin loads an m3u list with movie links and adds them to the movie directory.";

        /// <summary>
        /// Gets the unique identifier of the plugin.
        /// </summary>
        public override Guid Id => new Guid("B638C373-BF90-4015-AE6D-C88F2735497D");

        /// <summary>
        /// Returns the default configuration for the plugin.
        /// </summary>
        /// <returns>The default PluginConfiguration.</returns>
        public PluginConfiguration GetDefaultConfiguration()
        {
            return new PluginConfiguration();
        }

        /// <summary>
        /// Returns the current configuration instance for the plugin.
        /// </summary>
        /// <returns>The PluginConfiguration instance.</returns>
        public object GetConfigurationInstance()
        {
            return _configuration;
        }

        /// <summary>
        /// Loads the M3U playlist, parses its entries, and adds movies to the library.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadM3UPlaylist()
        {
            var filePath = _configuration.M3UFilePath;
            var entries = M3UProcessor.ParseM3U(filePath);

            foreach (var entry in entries)
            {
                await AddMovieToLibrary(entry.Title, entry.Url).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adds a movie to the library based on the given title and URL.
        /// </summary>
        /// <param name="title">The title of the movie.</param>
        /// <param name="url">The URL of the movie.</param>
        private Task AddMovieToLibrary(string title, string url)
        {
            var movie = new Movie
            {
                Name = title,
                Path = url,
                IsVirtualItem = true
            };

            _libraryManager.CreateItem(movie, _libraryManager.GetUserRootFolder());
            _logger.LogInformation("Dodano film: {MovieName}", movie.Name);

            return Task.CompletedTask;
        }
    }
}
