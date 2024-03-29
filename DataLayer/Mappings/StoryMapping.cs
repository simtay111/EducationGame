﻿using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class StoryMapping : ClassMap<Story>
    {
        public StoryMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Summary).Length(4000);
            Map(x => x.MessageLessonText);
            Map(x => x.IsPublic);
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}