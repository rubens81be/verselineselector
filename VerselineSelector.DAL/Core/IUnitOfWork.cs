using VerselineSelector.DAL.Repositories;

namespace VerselineSelector.DAL.Core;

public interface IUnitOfWork : IDisposable
{
    IVerselineRepository Verselines { get; }

    IParagraphRepository Paragraphs { get; }

    IPatientRepository Patients { get; }

    int Complete();
}
