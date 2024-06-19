// File: Data/IMarketplaceUserRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using Backengv2.Models;

public interface IMarketplaceUserRepository
{
    // Obtiene una lista de usuarios del marketplace por sus IDs de forma asincrónica
    Task<List<MarketplaceUser>> GetMarketplaceUsersByIdsAsync(List<int> marketplaceUserIds);

    // Obtiene un usuario del marketplace por su ID de forma asincrónica
    Task<MarketplaceUser> GetByIdAsync(int id);
}
