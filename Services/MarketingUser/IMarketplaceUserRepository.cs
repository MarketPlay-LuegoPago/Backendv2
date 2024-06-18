// File: Data/IMarketplaceUserRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Backengv2.Models;

public interface IMarketplaceUserRepository
{
    Task<List<MarketplaceUser>> GetMarketplaceUsersByIdsAsync(List<int> marketplaceUserIds);
    Task<MarketplaceUser> GetByIdAsync(int id);
}