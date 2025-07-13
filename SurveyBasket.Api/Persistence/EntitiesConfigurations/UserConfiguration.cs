namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        

        builder.Property(p => p.FirstName).HasMaxLength(100);

        builder.Property(p => p.LastName).HasMaxLength(100);

;
    }
}
