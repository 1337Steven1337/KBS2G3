﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Server.Models.Context
{
    public class ServerContext : DbContext, IDocentAppContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ServerContext() : base("name=ServerContext")
        {

        }

        public DbSet<Server.Models.Question> Questions { get; set; }
        public DbSet<Server.Models.List> Lists { get; set; }
        public DbSet<Server.Models.UserAnswer> UserAnswers { get; set; }
        public DbSet<Server.Models.PredefinedAnswer> PredefinedAnswers { get; set; }

        public void MarkAsModified(Question item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(List item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(UserAnswer item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(PredefinedAnswer item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}