using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wordly.Core.Models.Entities;

namespace Wordly.DataAccess.EntityConfigurations;

public class CollectionsConfiguration : EntityConfigurationBase<Collection, Guid>
{
    public override void Configure(EntityTypeBuilder<Collection> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.Name).IsRequired();

        builder.HasOne(entity => entity.Author)
            .WithMany()
            .HasForeignKey(entity => entity.AuthorId);

            builder.HasMany(entity => entity.CollectionTerms)
            .WithOne(entity => entity.Collection)
            .HasForeignKey(entity => entity.CollectionId);
    }
}