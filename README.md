# CoreAudit
## What is CoreAudit?
CoreAudit is a small library for building an auditing system in C# applications. The README will be filled out over time however it provides you with all the structs necessary to track actions taken by users or other entities in your application.

Implement the IAuditManager interface and register it with your DI framework to use easily. This library is meant to be database agnostic and so you will need to provide your own DAO.

```CSharp
public enum SubjectType
{
  ModelA,
  ModelB,
  ModelC
}

public enum ActionType
{
  CreatedModelA,
  EditedModelA,
  DeletedModelA
}

public class ExampleSubject : ICoreAuditSubject<SubjectType, ActionType>
{
  public int UserId;
  public SomeModel Model;
  public ActionType Action;
  
  // put the data needed to build your log message here.
  public SubjectType Type() => SubjectType.CreatedModelA;
  public string BuildMessage => $"{UserId} {Action.ToString()} {Model.ToString}"; /* Interpolate your data and return it here. */
}

public class AuditEntry : AuditEntryBase<ObjectId, SubjectType>
{
}

public class AuditManager : IAuditManager<AuditEntry, ObjectId, SubjectType>
{
  private readonly MongoFactory _factory;
  
  public AuditManager(MongoFactory factory)
  {
    _factory = factory;
  }
  
  public void Audit(ICoreAuditSubject<SubjectType> subject)
  {
    // Your DAO will look different. So long as an AuditEntry item is created and passed your subject class.
    _factory.AuditEntry.Create(new AuditEntry
    {
      Type = subject.Type(),
      Message = subject.BuildMessage()
    });
  }
  
  public List<AuditEntry> Report(SubjectType? type)
  {
    if (type == null)
      return _factory.AuditEntry.Get();
      
    return _factory.AuditEntry.Find(x => x.Type == type);
  }
}

public class ExampleController : Controller
{
  private readonly IAuditManager<AuditEntry, ObjectId, SubjectType>  _auditManager;
  
  // Dependencies provided by Dependency Injection
  public ExampleController(IAuditManager<AuditEntry, ObjectId, SubjectType> auditManager)
  {
    _auditManager = auditManager;
  }
  
  [HttpPost]
  public IActionResult ExampleCrudEndpoint([FromBody] SomeModel updatedModel)
  {
    // do your stuff and then audit the action:
    _auditManager.Audit(new ExampleSubject
    {
      UserId = currentUser.Id,
      Model = updatedModel,
      Action = ActionType.EditedModelA
    });
  }
}
```
