using System;
using System.Collections.Generic;
using CoreAudit;

namespace CoreAuditTestApp
{
    public enum SubjectType
    {
        ModelA,
        ModelB
    }

    public enum ActionType
    {
        CreatedModelA,
        EditedModelA,
        DeletedModelA
    }

    public class ExampleSubject : ICoreAuditSubject<SubjectType>
    {
        public string BuildMessage()
        {
            throw new NotImplementedException();
        }

        public SubjectType Type()
        {
            throw new NotImplementedException();
        }
    }

    public class AuditEntry : AuditEntryBase<int, SubjectType> {}

    public class TestCollection
    {
        public void Create(AuditEntry entry)
        {
            
        }
    }
    public class TestDao
    {
        public TestCollection Collection => new TestCollection();
    }

    public class AuditManager : IAuditManager<AuditEntry, int, SubjectType>
    {
        private readonly IDatabaseWrapper<TestDao> _database;
        public void Audit(ICoreAuditSubject<SubjectType> subject)
        {
            _database.Database.Collection.Create(new AuditEntry
            {
                Type = subject.Type(),
                Message = subject.BuildMessage()
            });
        }

        public List<AuditEntry> Report(SubjectType type)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var auditManager = new AuditManager();

            auditManager.Audit(new ExampleSubject
            {

            });
        }
    }
}
