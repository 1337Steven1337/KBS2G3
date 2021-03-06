﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.DTO
{
    public class PredefinedAnswerDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Right_Answer { get; set; }
        public int Question_Id { get; set; }

        public PredefinedAnswerDTO() { }

        public PredefinedAnswerDTO(PredefinedAnswer predefinedAnswer)
        {
            this.Id = predefinedAnswer.Id;
            this.Text = predefinedAnswer.Text;
            this.Right_Answer = predefinedAnswer.Right_Answer;
            this.Question_Id = predefinedAnswer.Question_Id;
        }
    }
}