// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Trade.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		#endregion


		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}
        private static string authLoginToken = string.Empty;
        public static string AuthLoginToken
        {
            get => authLoginToken;
            set => authLoginToken = value;
        }

        private static string authUserName = string.Empty;
        public static string AuthUserName
        {
            get => authUserName;
            set => authUserName = value;
        }

        private static int notificaton;
        public static int Notificaton
        {
            get => notificaton;
            set => notificaton = value;
        }

    }
}