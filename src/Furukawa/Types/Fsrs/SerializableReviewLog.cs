﻿using System.Globalization;
using FSRSharp;
using Realms;

namespace Furukawa.Types;

public partial class SerializableReviewLog : IRealmObject
{
    public Guid Guid { get; set; }
    public int Rating { get; set; } // (int)FsrsCardRating
    public int ScheduledDays { get; set; }
    public int ElapsedDays { get; set; }
    public string Review { get; set; } // (string)DateTime
    public int State { get; set; } // (int)FsrsCardState
        
    public static SerializableReviewLog FromReviewLog(ReviewLog review, Guid guid)
    {
        return new SerializableReviewLog()
        {
            Guid = guid,
            Rating = (int)review.Rating,
            ScheduledDays = review.ScheduledDays,
            ElapsedDays = review.ElapsedDays,
            Review = review.Review.ToString("O"),
            State = (int)review.State
        };
    }
        
    public ReviewLog ToReviewLog()
    {
        return new ReviewLog((CardRating)this.Rating, this.ScheduledDays, this.ElapsedDays,
            DateTime.Parse(this.Review, null, DateTimeStyles.RoundtripKind), (CardState)this.State);
    }
}