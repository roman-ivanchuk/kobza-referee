﻿using KobzaReferee.Domain.Entities._Base;

namespace KobzaReferee.Domain.Entities;

public class TelegramChat : EntityBase
{
    public string Type { get; set; } = string.Empty;

    public string? Title { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? IsForum { get; set; }
}
