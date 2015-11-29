﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.Controller;
using Client.View.PanelLayout;
using Client.Model;

namespace Client.View.Question
{
    public partial class ViewQuestion : Form, IQuestionView
    {
        private QuestionController controller;
        private CustomPanel customPanel;

        public int listId { get; set;}

        public ViewQuestion()
        {
            InitializeComponent();
            customPanel = new CustomPanel();
        }

        public void setController(QuestionController controller)
        {
            this.controller = controller;
        }

        public ListBox getListBox()
        {
            return listBoxQuestion;
        }

        public void fillList(List<Model.Question> list)
        {
            foreach (var item in list)
            {
                if(item.List_Id == listId)
                {
                    listBoxQuestion.DataSource = list;
                    listBoxQuestion.DisplayMember = "Text";
                    listBoxQuestion.ValueMember = "Id";
                }
            }
        }

        public CustomPanel getCustomPanel()
        {
            return customPanel;
        }
    }
}
