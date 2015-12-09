﻿using Client.View.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Controller;
using Client.Model;
using Client.Service.Thread;
using Client.View;
using System.Windows.Forms;
using Client.Controller.Question;

namespace Client.Tests.View.Question
{
    class TestViewQuestion : IListView<Model.Question>
    {
        private List<Model.Question> Questions = new List<Model.Question>();

        public void AddItem(Model.Question item)
        {
            throw new NotImplementedException();
        }

        public void AddToParent(IView parent)
        {
            throw new NotImplementedException();
        }

        public void FillList(List<Model.Question> list)
        {
            foreach (Model.Question item in list)
            {
                Questions.Add(item);
            }
        }

        public IControlHandler GetHandler()
        {
            return null;
        }

        public void SetController(IController controller)
        {
            //Should be implemented when the controller is needed
        }
    }
}
