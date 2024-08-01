using DevExpress.Xpf.Core;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using TierType = Hisba.Data.Layers.EntitiesInfo.TierType;

namespace Hisba.Data.Bll.Entities
{
    public class TierBll
    {
        public async static void Add(Tier Tier)
        {
            try
            {
                using(var context = new AppDbContext())
                {
                    context.Tiers.Add(Tier);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async static void Update(Tier Tier)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var oldTier = await context.Tiers.FindAsync(Tier.Id);

                    if (oldTier == null) return;

                    oldTier.Id = Tier.Id;
                    oldTier.Code = Tier.Code;
                    oldTier.Reference = Tier.Reference;
                    oldTier.Name = Tier.Name;
                    oldTier.FirstName = Tier.FirstName;
                    oldTier.LastName = Tier.LastName;
                    oldTier.BirthDate = Tier.BirthDate;
                    oldTier.PhoneNumber = Tier.PhoneNumber;
                    oldTier.Email = Tier.Email;
                    oldTier.Address = Tier.Address;
                    oldTier.Commune = Tier.Commune;
                    oldTier.Wilaya = Tier.Wilaya;
                    oldTier.Type = Tier.Type;
                    oldTier.CreatorId = Tier.CreatorId;
                    oldTier.Created = Tier.Created;
                    oldTier.ModifierId = Tier.ModifierId;
                    oldTier.Modified = Tier.Modified;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        public async static void Delete(Tier Tier)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var TierToDelete = await context.Tiers.FindAsync(Tier.Id);

                    if (TierToDelete == null) return;

                    context.Tiers.Remove(TierToDelete);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public static async Task<Tier> GetTierById(int id)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var tier = await context.Tiers.FindAsync(id);
                    return tier;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<TierInfo> GetClient(string Reference)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var client = await context.Tiers.Where(t => t.Type == Layers.Entities.TierType.Client).Select(item => new TierInfo
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Reference = item.Reference,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        BirthDate = (DateTime)item.BirthDate,
                        PhoneNumber = item.PhoneNumber,
                        Address = item.Address,
                        Wilaya = item.Wilaya,
                        Commune = item.Commune,
                        Email = item.Email,
                    }).FirstOrDefaultAsync();
                    return client;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }            
        }

        public static async Task<TierInfo> GetProvider(string Reference)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var provider = await context.Tiers.Where(t => t.Type == Layers.Entities.TierType.Provider).Select(item => new TierInfo
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Reference = item.Reference,
                        Name = item.Name,
                        PhoneNumber = item.PhoneNumber,
                        Address = item.Address,
                        Wilaya = item.Wilaya,
                        Commune = item.Commune,
                        Email = item.Email,
                    }).FirstOrDefaultAsync();
                    return provider;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }           
        }

        public static async Task<List<Tier>> GetAllTiers()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var tiers = await context.Tiers.ToListAsync();
                    return tiers;
                } 
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<TierInfo>> GetAllProviders()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var providers = await context.Tiers.Where(t => t.Type == Layers.Entities.TierType.Provider).Select(item => new TierInfo
                    {
                        Code = item.Code,
                        Reference = item.Reference,
                        Name = item.Name,
                        PhoneNumber = item.PhoneNumber,
                        Address = item.Address,
                        Wilaya = item.Wilaya,
                        Commune = item.Commune,
                        Email = item.Email,
                    }).ToListAsync();
                    return providers;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }

        public static async Task<List<TierInfo>> GetAllClients()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var clients = await context.Tiers.Where(t => t.Type == Layers.Entities.TierType.Client).Select(item => new TierInfo
                    {
                        Code = item.Code,
                        Reference = item.Reference,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        BirthDate = (DateTime)item.BirthDate,
                        PhoneNumber = item.PhoneNumber,
                        Address = item.Address,
                        Wilaya = item.Wilaya,
                        Commune = item.Commune,
                        Email = item.Email,
                    }).ToListAsync();
                    return clients;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }
    }
}
