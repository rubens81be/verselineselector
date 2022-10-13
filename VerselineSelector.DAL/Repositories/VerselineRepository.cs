using VerselineSelector.DAL.Core;
using VerselineSelector.Domain.ChapterIV;

namespace VerselineSelector.DAL.Repositories;

public class VerselineRepository : EfRepository<VerselineEntity>, IVerselineRepository
{
    public VerselineRepository(DatabaseContext dBContext) : base(dBContext)
    {
    }

    public async Task<IEnumerable<VerselineEntity>> GetAllForParagraph(string paragraph)
    {
        var query = Sam.Verselines.Where(c => c.ParagraphName == paragraph).OrderBy(c => c.Sequence);
        return await query.ToListAsync();
    }

    public DatabaseContext Sam => (DatabaseContext)_dBContext;
}
