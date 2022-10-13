using VerselineSelector.DAL.Core;
using VerselineSelector.Domain.ChapterIV;

namespace VerselineSelector.DAL.Repositories;

public class ParagraphRepository : EfRepository<ParagraphEntity>, IParagraphRepository
{
    public DatabaseContext Sam => (DatabaseContext)_dBContext;
    public ParagraphRepository(DatabaseContext dBContext) : base(dBContext) { }
    public async Task<IEnumerable<ParagraphEntity>> GetAllSortedAsc()
    {
        var query = Sam.Paragraphs.OrderBy(p => p.Name);
        return await query.ToListAsync();
    }

}
