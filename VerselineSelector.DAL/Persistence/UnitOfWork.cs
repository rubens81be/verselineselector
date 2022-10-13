using VerselineSelector.DAL.Core;
using VerselineSelector.DAL.Repositories;

namespace VerselineSelector.DAL.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _db;
    public UnitOfWork(DatabaseContext db)
    {
        _db = db;
        Verselines = new VerselineRepository(_db);
        Paragraphs = new ParagraphRepository(_db);
        Patients = new PatientRepository(_db);
    }

    public IVerselineRepository Verselines { get; private set; }

    public IParagraphRepository Paragraphs { get; private set; }

    public IPatientRepository Patients { get; private set; }

    public int Complete()
    {
        return _db.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _db.Dispose();
        }
    }
}
