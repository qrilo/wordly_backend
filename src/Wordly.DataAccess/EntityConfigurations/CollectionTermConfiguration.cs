using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wordly.Core.Models.Entities;

namespace Wordly.DataAccess.EntityConfigurations;

public class CollectionTermConfiguration : EntityConfigurationBase<CollectionTerm, Guid>
{
    public override void Configure(EntityTypeBuilder<CollectionTerm> builder)
    {
        base.Configure(builder);

        builder.Property(entity => entity.CollectionId).IsRequired();
        builder.Property(entity => entity.TermId).IsRequired();

        builder.HasOne(entity => entity.Term)
            .WithMany()
            .HasForeignKey(entity => entity.TermId);

        builder.HasOne(entity => entity.Collection)
            .WithMany(entity => entity.CollectionTerms)
            .HasForeignKey(entity => entity.CollectionId);
    }
}