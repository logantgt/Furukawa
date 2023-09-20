using System.Globalization;
using Furukawa.Models;
using FSRSharp;
using Realms;

namespace Furukawa.Types
{
    public partial class FsrsRealmCard : IRealmObject
    {
        [PrimaryKey]
        public Guid Guid { get; set; }
        public string Owner { get; set; }
        public IList<string> Decks { get; }
        public string ContentFile { get; set; }
        public string Due { get; set; } // (string)DateTime
        public float Stability { get; set; }
        public float Difficulty { get; set; }
        public int ElapsedDays { get; set; }
        public int ScheduledDays { get; set; }
        public int Reps { get; set; }
        public int Lapses { get; set; }
        public int State { get; set; } = 0; // (int)FsrsCardState
        public string LastReview { get; set; } // (string)DateTime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card">The card to derive FSRS parameters from.</param>
        /// <returns>Returns a new instance of FsrsRealmCard with this FsrsRealmCard's Guid, Owner, Decks, and ContentFile using the FSRS card parameters of the given Card.</returns>
        public FsrsRealmCard UpdateCard(Card card)
        {
            FsrsRealmCard output = new FsrsRealmCard
            {
                Guid = this.Guid,
                Owner = this.Owner,
                ContentFile = this.ContentFile,
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

            foreach (string deck in this.Decks)
            {
                output.Decks.Add(deck);
            }

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
}