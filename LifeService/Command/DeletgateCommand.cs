using System;
using System.Windows.Input;

namespace LifeService.Command
{
    internal class DeletgateCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod = null;
        private readonly Func<T, bool> _canExecuteMethod = null;
        private ICommand loaded;
        private object canLoaded;

        public DeletgateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        { }

        /// <summary>
        /// 委托命令
        /// </summary>
        /// <param name="executeMethod"></param>
        /// <param name="canExecuteMethod"></param>
        public DeletgateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException("executeMetnod");
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public DeletgateCommand(ICommand loaded, object canLoaded)
        {
            this.loaded = loaded;
            this.canLoaded = canLoaded;
        }

        #region ICommand 成员
        /// <summary>
        /// 判定是否可被执行的方法
        /// </summary>
        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            return true;

        }

        /// <summary>
        ///  执行命令
        /// </summary>
        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        #endregion


        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region ICommand 成员

        public bool CanExecute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
            {
                return (_canExecuteMethod == null);

            }

            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion
    }
}
