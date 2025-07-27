namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {

        builder.HasIndex(q => new { q.PollId, q.Content }).IsUnique();

        builder.Property(q => q.Content).HasMaxLength(1000);
    }
}
