using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;


namespace Sunplus.Accounting.Core.Extensions
{
    using System.Web;

    /**
     * public class Log Extensions
     * @Author: bobby.nguyen
     * @Date: 11012011
     **/
    public static class LogExtensions
    {
        private static BackgroundWorker _worker = null;
        private static Queue<Exception> _queueEx = new Queue<Exception>();
        /**
         * Function: Exception Handler
         */
        public static void ExceptionHandler(Exception ex)
        {
           /* _queueEx.Enqueue(ex);
            using (_worker = new BackgroundWorker())
            {
                _worker.WorkerSupportsCancellation = true;
                _worker.DoWork += new DoWorkEventHandler(WorkerRunning);
                _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
                _worker.RunWorkerAsync();
            }*/

            //if (ex.GetType() == typeof(HttpException))
            //{
                
            //}

            Fanex.Logging.Logger.Log.Error(ex.Message, ex);
        }
        /**
         * Function: BackgroundWorker running
         */
        private static void WorkerRunning(object sender, DoWorkEventArgs e)
        {
            if (_queueEx.Count > 0)
            {
                var ex = _queueEx.Dequeue();
                
                Fanex.Logging.Logger.Log.Error(ex.Message, ex);
            }
            else
            {
                _worker.CancelAsync();
            }
        }
        /**
         * Function: BackgroundWorker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO
        }
    }
}
