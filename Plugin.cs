﻿using IllusionPlugin;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Claws")]
[assembly: AssemblyFileVersion("1.0.0")]
[assembly: AssemblyCopyright("MIT License - Copyright © 2019 Steffan Donal")]

[assembly: Guid("a563479b-6b8d-41f0-9a23-cdc396dd9cf0")]

namespace Claws
{
    public class Plugin : IPlugin
    {
        static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        public static readonly string Name = Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        public static readonly string Version = Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

        /// <summary>
        /// True if the mod is enabled in-game.
        /// </summary>
        public static bool IsEnabled
        {
            get => _isEnabled;
            internal set
            {
                if (value == _isEnabled) return;

                _isEnabled = value;
                IsEnabledChanged?.Invoke(typeof(Plugin), EventArgs.Empty);
            }
        }
        static bool _isEnabled;

        /// <summary>
        /// Invoked whenever the plugin's enabled state changes.
        /// </summary>
        public static event EventHandler IsEnabledChanged;


        void IPlugin.OnApplicationStart()
        {
            Log($"v{Version} loaded!");
        }

        void IPlugin.OnApplicationQuit()
        {

        }


        internal static void Log(string message)
        {
            Console.WriteLine($"[{Name}] {message}");
        }


        #region Unused IPlugin Members

        string IPlugin.Name => Name;
        string IPlugin.Version => Version;

        void IPlugin.OnUpdate() { }
        void IPlugin.OnFixedUpdate() { }
        void IPlugin.OnLevelWasLoaded(int level) { }
        void IPlugin.OnLevelWasInitialized(int level) { }

        #endregion
    }
}
