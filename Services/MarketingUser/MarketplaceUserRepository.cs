using Backengv2.Data;
using Backengv2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MarketplaceUserRepository : IMarketplaceUserRepository
{
    private readonly BaseContext _context;

    public MarketplaceUserRepository(BaseContext context)
    {
        _context = context;
    }

    // Método para obtener una lista de usuarios del marketplace por sus IDs de forma asincrónica
    public async Task<List<MarketplaceUser>> GetMarketplaceUsersByIdsAsync(List<int> marketplaceUserIds)
    {
        return await _context.MarketplaceUser.Where(mu => marketplaceUserIds.Contains(mu.Id)).ToListAsync();
    }

    // Método para obtener un usuario del marketplace por su ID de forma asincrónica
    public async Task<MarketplaceUser> GetByIdAsync(int id)
    {
        return await _context.MarketplaceUser.SingleOrDefaultAsync(mu => mu.Id == id);
    }
}
