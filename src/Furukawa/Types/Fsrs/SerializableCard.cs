using System.Globalization;
using Furukawa.Models;
using FSRSharp;
using Realms;

namespace Furukawa.Types;

public partial class SerializableCard : IRealmObject
{
    [PrimaryKey] public Guid Guid { get; set; } = Guid.Empty;
    public Guid Owner { get; set; } = Guid.Empty;
    public string Note { get; set; } = ""; // SortField of a CorpusNote entry
    public bool Blacklisted { get; set; } = false;
    public string Due { get; set; } = ""; // (string)DateTime
    public float Stability { get; set; } = 0f;
    public float Difficulty { get; set; } = 0f;
    public int ElapsedDays { get; set; } = 0;
    public int ScheduledDays { get; set; } = 0;
    public int Reps { get; set; } = 0;
    public int Lapses { get; set; } = 0;
    public int State { get; set; } = 0; // (int)FsrsCardState
    public string LastReview { get; set; } = ""; // (string)DateTime

    /// <summary>
    /// Return the ownership values of this FsrsCard along with new values provided in a given Card, retaining the same Guid, Owner, Note, and Blacklisted values.
    /// </summary>
    /// <param name="card">The card to derive FSRS parameters from.</param>
    /// <returns>Returns a new instance of FsrsRealmCard with this FsrsRealmCard's Guid, Owner, Decks, and ContentFile using the FSRS card parameters of the given Card.</returns>
    public SerializableCard UpdateCard(Card card)
    {
        SerializableCard output = new SerializableCard
        {
            Guid = this.Guid,
            Owner = this.Owner,
            Note = this.Note,
            Blacklisted = this.Blacklisted,
            
            Due = card.Due.ToString("O"),
            Stability = card.Stability,
            Difficulty = card.Difficulty,
            ElapsedDays = card.ElapsedDays,
            ScheduledDays = card.ScheduledDays,
            Reps = card.Reps,
            Lapses = card.Lapses,
            State = (int)card.State,
            LastReview = card.LastReview.ToString("O")
        };
            
        return output;
    }

    /// <summary>
    /// Get the FSRS card parameters of this entry.
    /// </summary>
    /// <returns>Card with the parameters stored in this FsrsRealmCard.</returns>
    public Card ToCard()
    {
        return new Card
        {
            Due = DateTime.Parse(this.Due, null, DateTimeStyles.RoundtripKind),
            Stability = this.Stability,
            Difficulty = this.Difficulty,
            ElapsedDays = this.ElapsedDays,
            ScheduledDays = this.ScheduledDays,
            Reps = this.Reps,
            Lapses = this.Lapses,
            State = (CardState)this.State,
            LastReview = DateTime.Parse(this.LastReview, null, DateTimeStyles.RoundtripKind)
        };
    }
}