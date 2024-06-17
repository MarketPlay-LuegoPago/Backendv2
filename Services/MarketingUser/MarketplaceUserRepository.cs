
using Backengv2.Data;
using Backengv2.Models;
using Microsoft.EntityFrameworkCore;

public class MarketplaceUserRepository : IMarketplaceUserRepository
{
    private readonly BaseContext _context;

    public MarketplaceUserRepository(BaseContext context)
    {
        _context = context;
    }

    public async Task<List<MarketplaceUser>> GetMarketplaceUsersByIdsAsync(List<int> marketplaceUserIds)
    {
        return await _context.MarketplaceUser.Where(mu => marketplaceUserIds.Contains(mu.Id)).ToListAsync();
    }

    public async Task<MarketplaceUser> GetByIdAsync(int id)
    {
        return await _context.MarketplaceUser.SingleOrDefaultAsync(mu => mu.Id == id);
    }
}
