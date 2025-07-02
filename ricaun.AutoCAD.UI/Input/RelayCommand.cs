using System;
using System.Windows.Input;

namespace ricaun.AutoCAD.UI.Input
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                return false;
            }

            //if (!string.IsNullOrEmpty(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.CommandInProgress))
            //    return false;

            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                throw new InvalidOperationException("Parameter cannot be null for value types.");
            }

            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}