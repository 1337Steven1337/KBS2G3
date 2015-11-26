﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Question
    {
        public int Id { get; private set; }
        public int Points { get; set; }
        public int Time { get; set; }
        public int List_Id { get; private set; }

        public string Text { get; set; }

        public QuestionList QuestionList { get; set; }
    }
}