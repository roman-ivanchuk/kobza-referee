namespace KobzaReferee.Persistence.Sqlite;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TelegramUser> TelegramUsers => Set<TelegramUser>();
    public DbSet<TelegramChat> TelegramChats => Set<TelegramChat>();
    public DbSet<WordGuess> WordGuesses => Set<WordGuess>();
    public DbSet<GuessDetail> GuessDetails => Set<GuessDetail>();
    public DbSet<LetterDetail> LetterDetails => Set<LetterDetail>();
    public DbSet<TournamentStatistics> TournamentStatistics => Set<TournamentStatistics>();
    public DbSet<TournamentParticipantStatistics> TournamentParticipantStatistics => Set<TournamentParticipantStatistics>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TelegramUserConfiguration());
        modelBuilder.ApplyConfiguration(new TelegramChatConfiguration());
        modelBuilder.ApplyConfiguration(new WordGuessConfiguration());
        modelBuilder.ApplyConfiguration(new GuessDetailConfiguration());
        modelBuilder.ApplyConfiguration(new LetterDetailConfiguration());
        modelBuilder.ApplyConfiguration(new TournamentStatisticsConfiguration());
        modelBuilder.ApplyConfiguration(new TournamentParticipantStatisticsConfiguration());
    }
}
