using System;
using System.Collections.Generic;

namespace DomainLayer.Entities
{
    public class SystemState 
    {
        public virtual int Id { get; set; }

        public virtual DateTime LastDateChecked { get; set; }
    }
}