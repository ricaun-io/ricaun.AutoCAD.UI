﻿using Autodesk.Windows;
using System.Windows.Input;

namespace ricaun.AutoCAD.UI.Windows
{
    /// <summary>
    /// Provides methods to display custom message boxes in AutoCAD using WPF windows.
    /// </summary>
    public static class MessageBox
    {
        /// <summary>
        /// Displays a message box showing the exception type and message.
        /// </summary>
        /// <param name="ex">The exception to display.</param>
        public static void ShowException(System.Exception ex)
        {
            ShowMessage(ex.GetType().Name, ex.Message);
        }

        /// <summary>
        /// Displays a message box with the specified title and message.
        /// </summary>
        /// <param name="title">The title of the message box window.</param>
        /// <param name="message">The message to display in the message box.</param>
        public static void ShowMessage(string title, string message)
        {
            var window = new System.Windows.Window()
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                MinWidth = 200,
                ResizeMode = System.Windows.ResizeMode.NoResize,
                SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                Title = title ?? nameof(MessageBox),
                ShowInTaskbar = false,
                Topmost = true,
                Content = new System.Windows.Controls.TextBlock()
                {
                    MaxWidth = 400,
                    Margin = new System.Windows.Thickness(15),
                    TextWrapping = System.Windows.TextWrapping.WrapWithOverflow,
                    Text = message,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                },
            };
            window.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, e) => { if (s is System.Windows.Window w) w.Close(); }));
            window.InputBindings.Add(new InputBinding(ApplicationCommands.Close, new KeyGesture(Key.Escape)));
            new System.Windows.Interop.WindowInteropHelper(window) { Owner = ComponentManager.ApplicationWindow };
            window.Show();
        }

        /// <summary>
        /// Displays a message box with the specified message.
        /// </summary>
        /// <param name="message">The message to display in the message box.</param>
        public static void ShowMessage(string message)
        {
            ShowMessage(null, message);
        }
    }
}