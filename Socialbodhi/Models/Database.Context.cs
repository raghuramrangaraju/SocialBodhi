﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Socialbodhi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SocialbodhiEntities : DbContext
    {
        public SocialbodhiEntities()
            : base("name=SocialbodhiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Instance> Instances { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
    }
}
