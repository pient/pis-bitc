using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Castle.ActiveRecord;
using PostSharp.Aspects;

namespace PIC.Data
{
    /// <summary>
    /// 事务处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false), Serializable]
    public class ActiveRecordTransactionAttribute : OnMethodBoundaryAspect
    {
        private TransactionMode transactionMode = TransactionMode.Inherits;
        private IsolationLevel isolationLevel = IsolationLevel.Unspecified;
        private OnDispose onDisposeBehavior = OnDispose.Commit;

        public ActiveRecordTransactionAttribute()
        {
        }

        public ActiveRecordTransactionAttribute(TransactionMode mode)
        {
            this.transactionMode = mode;
        }

        public ActiveRecordTransactionAttribute(IsolationLevel isolationLevel)
        {
            
            this.isolationLevel = isolationLevel;
        }

        public ActiveRecordTransactionAttribute(TransactionMode mode, IsolationLevel isolationLevel)
        {
            this.transactionMode = mode;
            this.isolationLevel = isolationLevel;
        }

        public ActiveRecordTransactionAttribute(TransactionMode mode, IsolationLevel isolationLevel, OnDispose onDisposeBehavior)
        {
            this.transactionMode = mode;
            this.isolationLevel = isolationLevel;
            this.onDisposeBehavior = onDisposeBehavior;
        }

        /// <summary>
        /// 设置事务模式
        /// </summary>
        public TransactionMode TransactionMode
        {
            get { return transactionMode; }
            set { transactionMode = value; }
        }

        /// <summary>
        /// 隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get { return isolationLevel; }
            set { isolationLevel = value; }
        }

        /// <summary>
        /// Dispose操作，默认Commit
        /// </summary>
        public OnDispose OnDisposeBehavior
        {
            get { return onDisposeBehavior; }
            set { onDisposeBehavior = value; }
        }

        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            TransactionScope trans = new TransactionScope(TransactionMode, IsolationLevel, OnDisposeBehavior);
            eventArgs.MethodExecutionTag = trans;

            base.OnEntry(eventArgs);
        }

        /// <summary>
        /// 出现异常
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            TransactionScope trans = eventArgs.MethodExecutionTag as TransactionScope;

            if (trans != null)
            {
                trans.VoteRollBack();
            }

            base.OnException(eventArgs);
        }

        /// <summary>
        /// 方法成功时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnSuccess(MethodExecutionArgs eventArgs)
        {
            TransactionScope trans = eventArgs.MethodExecutionTag as TransactionScope;

            if (trans != null)
            {
                trans.VoteCommit();
            }

            base.OnSuccess(eventArgs);
        }

        /// <summary>
        /// 退出时
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnExit(MethodExecutionArgs eventArgs)
        {
            TransactionScope trans = eventArgs.MethodExecutionTag as TransactionScope;

            if (trans != null)
            {
                trans.Dispose();
            }

            base.OnExit(eventArgs);
        }
    }
}
