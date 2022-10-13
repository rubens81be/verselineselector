using VerselineSelector.Domain.ChapterIV;

namespace VerselineSelector.DAL.Repositories;

public interface IVerselineRepository : IAsyncRepository<VerselineEntity>
{
    Task<IEnumerable<VerselineEntity>> GetAllForParagraph(string paragraph);
}
