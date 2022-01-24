using AmbientSounds.Constants;

namespace AmbientSounds.Services
{
    /// <summary>
    /// Allows programmatic page navigation.
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// The root frame of the app.
        /// </summary>
        object? RootFrame { get; set; }

        /// <summary>
        /// The inner frame that can navigate. This must be set before
        /// any method is called.
        /// </summary>
        object? ContentFrame { get; set; }

        /// <summary>
        /// Navigates to the given page.
        /// </summary>
        /// <param name="pageKey">A page key from <see cref="UIConstants"/>.</param>
        /// <param name="parameters">Parameters to pass to the navigation.</param>
        void ContentNavigate(string pageKey, object? parameters = null);

        /// <summary>
        /// Navigates to the screensaver.
        /// </summary>
        void ToScreensaver();

        /// <summary>
        /// Navigates to the compact page.
        /// </summary>
        void ToCompact();

        /// <summary>
        /// Navigates to the upload page.
        /// </summary>
        void ToUploadPage();

        /// <summary>
        /// Navigates to the catalogue page.
        /// </summary>
        void ToCatalogue();

        /// <summary>
        /// Attempts to navigate back.
        /// </summary>
        /// <param name="sourcePage">Optional. If provided,
        /// then specific go back functionality may be applied.
        /// For example, if ScreensaverPage is provided, the 
        /// RootFrame is used for GoBack.</param>
        void GoBack(string? sourcePage = null);
    }
}