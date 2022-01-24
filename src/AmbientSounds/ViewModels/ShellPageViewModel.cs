using AmbientSounds.Constants;
using AmbientSounds.Services;
using JeniusApps.Common.Models;
using JeniusApps.Common.Tools;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace AmbientSounds.ViewModels
{
    /// <summary>
    /// ViewModel for the shell page.
    /// </summary>
    public class ShellPageViewModel : ObservableObject
    {
        private readonly IUserSettings _userSettings;
        private readonly ITimerService _ratingTimer;
        private readonly ITelemetry _telemetry;
        private readonly INavigator _navigator;
        private bool _isRatingMessageVisible;

        public ShellPageViewModel(
            IUserSettings userSettings,
            ITimerService timer,
            ITelemetry telemetry,
            INavigator navigator,
            ILocalizer localizer,
            ISystemInfoProvider systemInfoProvider)
        {
            Guard.IsNotNull(userSettings, nameof(userSettings));
            Guard.IsNotNull(timer, nameof(timer));
            Guard.IsNotNull(telemetry, nameof(telemetry));
            Guard.IsNotNull(navigator, nameof(navigator));

            _userSettings = userSettings;
            _ratingTimer = timer;
            _telemetry = telemetry;
            _navigator = navigator;

            _userSettings.SettingSet += OnSettingSet;

            var lastDismissDateTime = _userSettings.GetAndDeserialize<DateTime>(UserSettingsConstants.RatingDismissed);
            if (!systemInfoProvider.IsFirstRun() &&
                !systemInfoProvider.IsTenFoot() &&
                !_userSettings.Get<bool>(UserSettingsConstants.HasRated) &&
                lastDismissDateTime.AddDays(7) <= DateTime.UtcNow)
            {
                _ratingTimer.Interval = 1800000; // 30 minutes
                _ratingTimer.IntervalElapsed += OnIntervalLapsed;
                _ratingTimer.Start();
            }

            MenuItemClickedCommand = new RelayCommand<MenuItem>(OnMenuItemClicked);
            MenuItems.Add(new MenuItem(MenuItemClickedCommand, localizer.GetString("Home"), "\uEA80", UIConstants.HomePageKey));
            MenuItems.Add(new MenuItem(MenuItemClickedCommand, localizer.GetString("Catalogue"), "\uE912", UIConstants.CataloguePageKey));
        }

        private void OnMenuItemClicked(MenuItem? menuItem)
        {
            if (menuItem is null)
            {
                return;
            }

            _navigator.ContentNavigate(menuItem.Tag ?? string.Empty);
        }

        private RelayCommand<MenuItem> MenuItemClickedCommand { get; }

        /// <summary>
        /// List of menu items. 
        /// </summary>
        public List<MenuItem> MenuItems = new();

        /// <summary>
        /// Determines if the rating message is visible.
        /// </summary>
        public bool IsRatingMessageVisible
        {
            get => _isRatingMessageVisible;
            set => SetProperty(ref _isRatingMessageVisible, value);
        }

        /// <summary>
        /// Path to background image.
        /// </summary>
        public string BackgroundImagePath => _userSettings.Get<string>(UserSettingsConstants.BackgroundImage);

        /// <summary>
        /// Determines if the background image should be shown.
        /// </summary>
        public bool ShowBackgroundImage => !string.IsNullOrWhiteSpace(BackgroundImagePath);

        public void Dispose()
        {
            _userSettings.SettingSet -= OnSettingSet;
        }

        private void OnIntervalLapsed(object sender, int e)
        {
            _ratingTimer.Stop();
            _ratingTimer.IntervalElapsed -= OnIntervalLapsed;
            IsRatingMessageVisible = true;
            _telemetry.TrackEvent(TelemetryConstants.RatingMessageShown);
        }

        private void OnSettingSet(object sender, string settingsKey)
        {
            if (settingsKey == UserSettingsConstants.BackgroundImage)
            {
                OnPropertyChanged(nameof(ShowBackgroundImage));
                OnPropertyChanged(nameof(BackgroundImagePath));
            }
        }
    }
}
