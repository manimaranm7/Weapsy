﻿using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles
{
    public class Role : AggregateRoot
    {
        public string Name { get; set; }
    }
}
