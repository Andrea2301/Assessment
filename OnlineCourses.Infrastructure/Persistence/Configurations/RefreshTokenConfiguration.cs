using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Token).IsUnique();
        builder.Property(x => x.Token).IsRequired();
    }
}