using System;
using System.Collections.Generic;
using CoreAudit;

namespace CoreAuditTestApp
{
    public enum SubjectTypes
    {
        Model1,
        Model2
    }

    public class ExampleSubject : ICoreAuditSubject<SubjectTypes>
    {
        public string BuildMessage()
        {
            throw new NotImplementedException();
        }

        public SubjectTypes Type()
        {
            throw new NotImplementedException();
        }
    }

    public class AuditEntry : AuditEntryBase<int, SubjectTypes> {}

    public class TestCollection
    {
        public void Create(AuditEntry entry)
        {

        }
    }
    public class TestDao
    {
        TestCollection GetCollection() => new TestCollection();
    }

    public class AuditManager : IAuditManager<AuditEntry, int, SubjectTypes>
    {
        IDatabaseWrapper<TestDao> _database;
        public void Audit(ICoreAuditSubject<SubjectTypes> subject)
        {
            // _database.Database.GetCollection();
            var entry = new AuditEntry
            {
                Type = subject.Type(),
                Message = subject.BuildMessage()
            };
        }

        public List<AuditEntry> Report(SubjectTypes type)
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
