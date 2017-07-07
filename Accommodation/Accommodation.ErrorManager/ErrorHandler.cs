using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Accommodation.ErrorManager
{
    public static class ErrorHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void HandleError(Exception ex)
        {
            StackTrace stackTrace = new StackTrace();
            logger.Error("There is an error on: {0}, Class: {1}, Method: {2}", ex.Message, (((System.Reflection.MemberInfo)(stackTrace.GetFrame(1).GetMethod())).DeclaringType).FullName, stackTrace.GetFrame(1).GetMethod().Name, ex);
        }
    }
}
