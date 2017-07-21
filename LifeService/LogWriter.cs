using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LifeService
{
    public class LogWriter
    {
        #region Instance
        private LogWriter()
        {

        }

        class UniqueInstance
        {
            internal static LogWriter instance;
            static UniqueInstance()
            {
                instance = new LogWriter();
            }
        }

        public static LogWriter Instance
        {
            get
            {
                return UniqueInstance.instance;
            }
        }
        #endregion

        public ILog Logger { get; private set; } = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);

        public void Debug(object message)
        {
            Logger.Debug(message);
        }

        public void Debug(Exception exception)
        {
            Logger.Debug(exception, exception);
        }

        public void Info(object message)
        {
            Logger.Info(message);
        }

        public void Info(Exception exception)
        {
            Logger.Info(exception, exception);
        }

        public void Error(object message)
        {
            Logger.Error(message);
        }

        public void Error(Exception exception)
        {
            Logger.Error(exception, exception);
        }



    }
}
