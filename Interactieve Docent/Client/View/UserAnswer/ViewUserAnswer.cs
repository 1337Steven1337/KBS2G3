﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Controller;

namespace Client.View.UserAnswer
{
    public partial class ViewUserAnswer : Form, IUserAnswerView
    {
        private UserAnswerController Controller;

        public ViewUserAnswer()
        {
            InitializeComponent();
        }

        public void setController(UserAnswerController Controller)
        {
            this.Controller = Controller
        }
    }
}