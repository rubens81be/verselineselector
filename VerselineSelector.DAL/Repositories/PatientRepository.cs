using VerselineSelector.DAL.Core;
using VerselineSelector.Domain.Patient;

namespace VerselineSelector.DAL.Repositories;

public class PatientRepository : EfRepository<PatientEntity>, IPatientRepository
{
    public DatabaseContext Sam => (DatabaseContext)_dBContext;
    public PatientRepository(DbContext dBContext) : base(dBContext) { }
}
