namespace SurveyBasket.Persistence.EntitiesConfigurations;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(v => new { v.Id, v.QuestionId })
            .IsUnique();
    }
}