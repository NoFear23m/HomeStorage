using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Model;
using Model.Interfaces;

namespace HomeStorage.DbContext
{
    public class StorageContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StorageContext()
        {
           
        }

        public StorageContext(string sqlConnectionString) : this(new DbContextOptionsBuilder<StorageContext>()
            .UseSqlServer(sqlConnectionString).Options)
        {
          
        }

        
        internal StorageContext(DbContextOptions options) : base(options)
        {
           

        }


        public virtual DbSet<Model.StorageModel.Setting> Settings { get;set; }
        public virtual DbSet<Model.StorageModel.Article> Articles { get;set; }
        public virtual DbSet<Model.StorageModel.ArticleAttribute> ArticleAttributes { get;set; }
        public virtual DbSet<Model.StorageModel.Attribute> Attributes { get;set; }
        public virtual DbSet<Model.StorageModel.Attachment> Attachments { get;set; }
        public virtual DbSet<Model.StorageModel.Store> Stores { get;set; }
        public virtual DbSet<Model.StorageModel.StoreInfo> StoreInfos { get;set; }
        








        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HomeStorage.App;Trusted_Connection=True;")
                        .ConfigureWarnings(warnings => warnings.Throw(CoreEventId.IncludeIgnoredWarning));  
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.StorageModel.Article>().HasOne(a => a.StoreInfo).WithOne(b => b.Article).HasForeignKey<Model.StorageModel.StoreInfo>(c => c.StoreInfoId);
            modelBuilder.Entity<Model.StorageModel.Article>().HasOne(a => a.StoreInfo).WithOne(b => b.Article).HasForeignKey<Model.StorageModel.Article>(c => c.ArticleId);
        }



        public override int SaveChanges()
        {
            UpdateEntitysBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateEntitysBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Detaches all of the EntityEntry objects that have been added to the ChangeTracker.
        /// </summary>
        public void DetachAll()
        {
            foreach (EntityEntry entityEntry in ChangeTracker.Entries().ToArray())
            {
                if (entityEntry.Entity != null)
                {
                    entityEntry.State = EntityState.Detached;
                }
            }
        }



        private void UpdateEntitysBeforeSaving()
        {
           var objectStateEntries = ChangeTracker.Entries()
          .Where(e => e.Entity is ModelBase && e.State != EntityState.Detached && e.State != EntityState.Unchanged).ToList();

            foreach (var entry in objectStateEntries)
            {
                if (!(entry.Entity is ModelBase entityBase)) continue;
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if (entityBase is ILogicalDelete delete)
                        {
                            entry.State = EntityState.Modified;

                            delete.DeletedTimestamp = DateTime.Now;
                            delete.DeletedFlag = true;
                            entry.Property(nameof(delete.DeletedTimestamp)).IsModified = true;
                            entry.Property(nameof(delete.DeletedFlag)).IsModified = true;
                        }
                        break;
                    case EntityState.Modified:

                        if (entityBase is ILogicalTimestamp timestamp)
                        {
                            timestamp.LastUpdateTimestamp = DateTime.Now;
                            entry.Property(nameof(timestamp.LastUpdateTimestamp)).IsModified = true;

                        }
                        if (entityBase is ILogicalDelete deleteedited)
                        {
                            string originalvalue = entry.Property(nameof(deleteedited.DeletedFlag)).OriginalValue.ToString();
                            string currentValue = entry.Property(nameof(deleteedited.DeletedFlag)).CurrentValue.ToString();
                            bool.TryParse(originalvalue, out var result);
                            bool.TryParse(currentValue, out var result1);
                            if (result != result1)
                            {
                                deleteedited.DeletedTimestamp = DateTime.Now;
                                entry.Property(nameof(deleteedited.DeletedTimestamp)).IsModified = true;
                            }
                        }
                        break;
                    case EntityState.Added:
                        if (entityBase is ILogicalTimestamp logicalTimestamp)
                        {
                            logicalTimestamp.CreationTimestamp = DateTime.Now;
                            logicalTimestamp.LastUpdateTimestamp = DateTime.Now;
                            entry.Property(nameof(logicalTimestamp.CreationTimestamp)).IsModified = true;
                            entry.Property(nameof(logicalTimestamp.LastUpdateTimestamp)).IsModified = true;
                        }
                        break;
                }

            }

        }



    }
}
