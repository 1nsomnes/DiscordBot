using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Core.Database
{
    public class DatabaseService
    {
        public static void AddInfraction(ulong userId, string severity, string description, ulong modId)
        {
            using (var context = new DatabaseModel())
            {
                Infraction infraction = new Infraction();
                infraction.userId = userId;
                infraction.severity = severity;
                infraction.description = description;
                infraction.modId = modId;

                infraction.id = context.Infractions.AsQueryable().Select(p => p.id).OrderBy(p => p).Last() + 1;
                infraction.creationDate = DateTime.UtcNow;
                infraction.modificationDate = DateTime.UtcNow;

                var entity = context.Infractions.Add(infraction);
                entity.State = EntityState.Added;

                context.SaveChanges();
            }
        }

        public static void RemoveInfraction(ulong infractionId)
        {
            using (var context = new DatabaseModel())
            {
                var result = context.Infractions.AsQueryable().Where(inf => inf.id == infractionId).First();

                if (result is null) return;

                var entity = context.Infractions.Remove(result);
                entity.State = EntityState.Deleted;

                context.SaveChanges();
            }
        }

        public static void EditInfraction(ulong infractionId, string description)
        {
            using (var context = new DatabaseModel())
            {
                var result = context.Infractions.AsQueryable().Where(inf => inf.id == infractionId).First();

                if (result is null) return;

                result.description = description;

                var entity = context.Infractions.Update(result);
                entity.State = EntityState.Modified;

                context.SaveChanges();
            }
        }
    }
}
