﻿namespace MidnightLizard.Schemes.Querier.Models
{
    public class PublicScheme : VersionedModel
    {
        public string Name { get; set; }
        public string Description { get; set; } = "There is no description for this color scheme.";
        public int Generation { get; set; }
        public ColorScheme ColorScheme { get; set; }

        public string PublisherId { get; set; }
        public string PublisherName { get; set; }
        public bool PublisherCommunity { get; set; }

        public int Likes { get; set; }
        public string[] LikedBy { get; set; }

        public int Favorites { get; set; }
        public string[] FavoritedBy { get; set; }
    }
}
