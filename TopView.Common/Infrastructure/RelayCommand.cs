using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TopView.Common.Infrastructure
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public bool CanExecute(T parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(T parameter) => _execute(parameter);
        public event EventHandler? CanExecuteChanged;
        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(TranslateParameter(parameter)) ?? true;
        public void Execute(object? parameter) => _execute(TranslateParameter(parameter)); 
        private T TranslateParameter(object? parameter)
        {
            if (parameter == null) return default;

            if (parameter is T t) return t;

            try
            {
                return (T)Convert.ChangeType(parameter, typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;

        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
