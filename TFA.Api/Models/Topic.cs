﻿namespace TFA.Api.Models
{
    public class Topic
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
