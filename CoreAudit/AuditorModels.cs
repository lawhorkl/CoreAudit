using System;
using System.Collections.Generic;

namespace CoreAudit
{
    public interface IIdentifier<TIndex>
    {
        TIndex Id { get; set; }
    }

    public interface ISubject
    {
        string BuildReportString();    
    }

    public abstract class AuditEntryBase<TIndex, TTypeEnum, TMessage> : IIdentifier<TIndex> where TTypeEnum : struct, IConvertible
    {
        public virtual TIndex Id { get; set; }
        public virtual TTypeEnum Type { get; set; }
        public virtual TMessage Message { get; set; }
    }

    public interface IAuditManager<TReport, TSubject, TIndex, TSubjectType, TMessage> where TSubject : ISubject where TReport : AuditEntryBase<TIndex, TSubjectType, TMessage> 
        where TSubjectType : struct, IConvertible
    {
        void Audit(TSubject subject);
        List<TReport> Report(TSubject type);
    }
}