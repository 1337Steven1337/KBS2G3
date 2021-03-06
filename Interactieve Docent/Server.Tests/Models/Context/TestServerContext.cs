﻿using Server.Models;
using Server.Models.Context;
using Server.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tests.Models.Context
{
    public class TestServerContext : IDocentAppContext
    {
        public TestServerContext()
        {
            this.Questions = new TestQuestionDbSet();
            this.OpenQuestions = new TestOpenQuestionDbSet();
            this.UserAnswers = new TestUserAnswerDbSet();
            this.UserAnswerToOpenQuestions = new TestUserAnswerToOpenQuestionDbSet();
            this.QuestionLists = new TestListDbSet();
            this.PredefinedAnswers = new TestPredefinedAnswerDbSet();
            this.Accounts = new TestAccountDbSet();
            this.Pincodes = new TestPincodeDbSet();
        }

        public DbSet<Server.Models.Question> Questions { get; set; }
        public DbSet<Server.Models.OpenQuestion> OpenQuestions { get; set; }
        public DbSet<Server.Models.QuestionList> QuestionLists { get; set; }
        public DbSet<Server.Models.UserAnswer> UserAnswers { get; set; }
        public DbSet<Server.Models.UserAnswerToOpenQuestion> UserAnswerToOpenQuestions { get; set; }
        public DbSet<Server.Models.PredefinedAnswer> PredefinedAnswers { get; set; }
        public DbSet<Server.Models.Account> Accounts { get; set; }
        public DbSet<Server.Models.Pincode> Pincodes { get; set; }

        public void MarkAsModified(Question item) { }
        public void MarkAsModified(OpenQuestion item) { }
        public void MarkAsModified(QuestionList item) { }
        public void MarkAsModified(UserAnswer item) { }
        public void MarkAsModified(UserAnswerToOpenQuestion item) { }
        public void MarkAsModified(PredefinedAnswer item) { }
        public void MarkAsModified(Account item) { }
        public void MarkAsModified(Pincode item) { }

        public int SaveChanges()
        {
            return 0;
        }

        public virtual Task<int> SaveChangesAsync()
        {
            Task<int> task = new Task<int>(new Func<int>(SaveChanges));
            task.Start();

            return task;
        }

        public void Dispose() { }
    }
}
