﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Factory;
using Client.Model;
using Client.View;
using Client.Service.SignalR;
using System.Net;

namespace Client.Controller
{
    class UserAnswerToOpenQuestionController : AbstractController<Model.Question>
    {
        #region Variables & Instances
        public List<string> Questions;
        private IResultView<Model.UserAnswerToOpenQuestion> View;
        private Model.OpenQuestion Question;
        private List<UserAnswerToOpenQuestion> Answers;

        private OpenQuestionFactory Factory = new OpenQuestionFactory();
        private UserAnswerToOpenQuestionFactory UserAnswerToOpenQuestionFactory = new UserAnswerToOpenQuestionFactory();
        
        private SignalRClient SignalRClient;
        #endregion

        #region Constructor
        public UserAnswerToOpenQuestionController(IResultView<Model.UserAnswerToOpenQuestion> view)
        {
            this.View = view;
            this.View.SetController(this);
            this.SignalRClient = SignalRClient.GetInstance();

            //add events
            //questionController.selectedIndexChanged += QuestionController_selectedIndexChanged;

            this.UserAnswerToOpenQuestionFactory.UserAnswerToOpenQuestionAdded += UserAnswerToOpenQuestionFactory_userAnswerToOpenQuestionAdded;

            LoadList();
            view.Show();
        }

        //do this is a student has answered a question
        private void UserAnswerToOpenQuestionFactory_userAnswerToOpenQuestionAdded(UserAnswerToOpenQuestion answer)
        {
            if (this.Question == null)
            {
                if (this.Answers == null)
                {
                    this.Answers = new List<UserAnswerToOpenQuestion>();
                }

                this.Answers.Add(answer);
            }
            else
            {
                if (answer.Question_Id == this.Question.Id)
                {
                    if (this.Question.UserAnswers == null)
                    {
                        this.Question.UserAnswers = new List<UserAnswerToOpenQuestion>();
                    }

                    this.Question.UserAnswers.Add(answer);

                    //update the panel 
                    this.View.GetHandler().Invoke((Action)Refresh);
                }
            }
        }
        #endregion

        #region Methodes
        private void FillList(List<Model.UserAnswerToOpenQuestion> userAnswers, HttpStatusCode status)
        {
            if (status == HttpStatusCode.OK && userAnswers != null)
            {
                this.View.FillList(userAnswers);
            }
        }

        public void LoadList()
        {
            this.UserAnswerToOpenQuestionFactory.FindAll(this.View.GetHandler(), this.FillList);
        }

        private void SetCurrentQuestion(Model.OpenQuestion q)
        {
            this.Question = q;
            if (this.Answers != null)
            {
                if (q.UserAnswers == null)
                {
                    q.UserAnswers = this.Answers.FindAll(x => x.Question_Id == q.Id);
                }
                else
                {
                    foreach (Model.UserAnswerToOpenQuestion ua in this.Answers)
                    {
                        if (ua.Question_Id == this.Question.Id)
                        {
                            if (q.UserAnswers.Find(x => x.Id == ua.Id) == null)
                            {
                                q.UserAnswers.Add(ua);
                            }
                        }
                    }
                }
            }

            this.Refresh();
        }

        //if another question is selected
        public void SetQuestion(Model.OpenQuestion q)
        {
            if (q != null)
            {

                if (q.UserAnswers == null)
                {
                    this.Factory.FindById(q.Id, this.View.GetHandler(), this.SetCurrentQuestion);
                }
                else
                {
                    this.Question = q;
                    this.View.GetHandler().Invoke((Action)Refresh);
                }
            }
        }

        public void Dispose()
        {
            this.UserAnswerToOpenQuestionFactory.UserAnswerToOpenQuestionAdded -= UserAnswerToOpenQuestionFactory_userAnswerToOpenQuestionAdded;
        }

        public void Close()
        {
            this.View.Close();
        }

        public void Refresh()
        {
            //redraw the panel
            if (this.Question.UserAnswers != null)
            {
                this.View.Make(this.Questions, this.Question.Text);
            }
        }

        public override IView GetView()
        {
            throw new NotImplementedException();
        }

        public override void SetView(IView view)
        {
            throw new NotImplementedException();
        }

        public override void SetBaseFactory(IFactory<Model.Question> baseFactory)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}