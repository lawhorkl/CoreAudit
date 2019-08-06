using System;
using System.Collections.Generic;

namespace CoreAudit
{
    public interface IIdentifier<TIndex>
    {
        TIndex Id { get; set; }
    }

    public interface ICoreAuditSubject<TSubjectType> where TSubjectType : struct, IConvertible
    {
        TSubjectType Type();
        string BuildMessage();    
    }

    public interface IDatabaseWrapper<TDataAccessObject>
    {
        TDataAccessObject Database { get; set; }
    }

    public abstract class AuditEntryBase<TIndex, TTypeEnum> : IIdentifier<TIndex> where TTypeEnum : struct, IConvertible
    {
        public virtual TIndex Id { get; set; }
        public virtual TTypeEnum Type { get; set; }
        public virtual string Message { get; set; }
    }

    public interface IAuditManager<TReport, TIndex, TSubjectType> where TReport : AuditEntryBase<TIndex, TSubjectType> 
        where TSubjectType : struct, IConvertible
    {
        void Audit(ICoreAuditSubject<TSubjectType> subject);
        List<TReport> Report(TSubjectType type);
    }
}