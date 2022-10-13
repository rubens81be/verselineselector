using VerselineSelector.Domain.ChapterIV;

namespace VerselineSelector.DAL.Repositories;

public interface IParagraphRepository : IAsyncRepository<ParagraphEntity>
{
    Task<IEnumerable<ParagraphEntity>> GetAllSortedAsc();
}
