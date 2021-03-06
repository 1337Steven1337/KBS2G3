﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Service.SignalR;
using Client.Factory;
using Microsoft.AspNet.SignalR.Client;
using System.Net;
using Client.View.Student;
using Client.View.Dialogs;
using Client.Service.Thread;

namespace Client.Controller
{
    public class StudentFormController
    {
        private Student.QuestionForm mainForm;
        private int currentQuestionIndex = -1;
        SignalRClient client;
        private StudentForm view;

        public StudentFormController(Student.QuestionForm mainform)
        {
            this.mainForm = mainform;
            view = mainForm.getView();

            //Connect (this) client to the session
            client = SignalRClient.GetInstance();
            client.ConnectionStatusChanged += Client_connectionStatusChanged;
            client.Connect();

            //Adds an event to the QuestionAdded function which is called when a new question is added by the teacher.
            QuestionFactory questionFactory = new QuestionFactory();
            questionFactory.QuestionAdded += Factory_questionAdded;



            //Adds an event to the QuestionListContinue function which is called when the teacher presses "Next" button for the next question.
            QuestionListFactory listFactory = new QuestionListFactory();
            listFactory.QuestionListContinue += LIFactory_continue;
            listFactory.QuestionListStarted += LIFactory_startList;
            listFactory.QuestionListStopped += LIFactory_stopped;


            //Adds an event to the PredefinedAnswerAdded function which is called for each PredefinedAnswer in the next question.
            PredefinedAnswerFactory PAFactory = new PredefinedAnswerFactory();
            PAFactory.PredefinedAnswerAdded += PAFactory_predefinedAnswerAdded;

            //Adds event, when an openquestion is added
            OpenQuestionFactory OpenQuestionFactory = new OpenQuestionFactory();
            OpenQuestionFactory.OpenQuestionAdded += openQuestionAdded;
        }

        //Adds openquestions to the openquestion list and goes to the nextquestion if the student is not Busy making one.
        private void openQuestionAdded(Model.OpenQuestion openQuestion)
        {
            this.mainForm.getQuestionList().OpenQuestions.Add(openQuestion);

            if(!mainForm.isBusy())
            {
                mainForm.Invoke((Action)delegate () { mainForm.goToNextQuestion(); });
            }
        }

        //This function is called when the teacher presses "Next Question"
        //This function calls the goToNextQuestion function in the QuestionForm.
        private void LIFactory_continue()
        {
            mainForm.Invoke((Action)delegate () { this.mainForm.goToNextQuestion(); });
        }

        //This function is called when the teacher presses "Stop list"
        //This function calls the clearQuestionLists
        private void LIFactory_stopped()
        {
            mainForm.Invoke((Action)delegate () { this.mainForm.stopQuestionList(); });
        }

        //This function will add the question to the questionlist underneath.
        private void addQuestionsCallbackHandler(List<Model.Question> question, HttpStatusCode status)
        {
            if (status!= HttpStatusCode.OK)
            {
                MessageBox.Show("Er ging wat mis met het ophalen van de vragen, probeer het opnieuw");
            }
            else
            {
                foreach (Model.Question q in question)
                {
                    if (q.List_Id == mainForm.getQuestionList().Id)
                    {
                        mainForm.Invoke((Action)delegate () { mainForm.getQuestionList().MCQuestions.Add(q);});
                    }
                }
                if (!mainForm.isBusy() && mainForm.getQuestionList().MCQuestions.Count > 0)
                {
                    mainForm.Invoke((Action)delegate () { mainForm.goToNextQuestion(); });
                }
            }
        }


        //This function will check if the list has been received correctly from the function underneath.
        private void listStartedCallbackHandler(Model.QuestionList list, HttpStatusCode status)
        {
            if (status != HttpStatusCode.OK)
            {
                MessageBox.Show("Er ging wat mis met het fetchen van de lijst, probeer het opnieuw.");
            }
            else
            {
                mainForm.getQuestionList().Id = list.Id;
                mainForm.getQuestionList().Name = list.Name;
                mainForm.getQuestionList().Ended = list.Ended;
                mainForm.getQuestionList().Position = list.Position;
                mainForm.getQuestionList().Ended = false;

                QuestionFactory questionFactory = new QuestionFactory();
                questionFactory.FindAll(mainForm.getView().GetHandler(),addQuestionsCallbackHandler);
            }
        }

        //This function will start a questionlist once the teacher starts a selected questionlist
        private void LIFactory_startList(int list,bool tempo) 
        {
            mainForm.setTempo(tempo);
            QuestionListFactory listFactory = new QuestionListFactory();
            listFactory.FindById(list,mainForm.getView().GetHandler(),listStartedCallbackHandler);            
            SignalRClient.GetInstance().SubscribeList(list);
            if (mainForm.getController().getCurrentQuestionIndex() > -1)
            {
                mainForm.getController().setCurrentQuestionIndex(-1);
                mainForm.getQuestionList().OpenQuestions.Clear();
                mainForm.getQuestionList().MCQuestions.Clear();
            }      
        }

        //Checks the HTTP response, if it is not Created then stop the questionList because the results are not valid anymore.
        private void saveMCAnswerCallBackHandler(Client.Model.UserAnswer ua, HttpStatusCode code)
        {
            if (code == HttpStatusCode.Created && ua != null)
            {
                //ShowSaveSucceed();
                //Dont show anything, it is really annoying if a dialog pops up every time.
            }
            else
            {
                ShowSaveFailed();
                //Return to main screen
            }
        }



        //Checks the HTTP response, if it is not Created then stop the questionList because the results are not valid anymore.
        private void saveOpenAnswerCallBackHandler(Client.Model.UserAnswerToOpenQuestion ua, HttpStatusCode code)
        {
            if (code == HttpStatusCode.Created && ua != null)
            {
                //ShowSaveSucceed();
                //Dont show anything, it is really annoying if a dialog pops up every time.
            }
            else
            {
                ShowSaveFailed();
                //Return to main screen
            }
        }



        public void ShowSaveFailed()
        {
            FailedDialogView failed = new FailedDialogView();
            failed.getLabelFailed().Text = "Het opslaan is mislukt! Probeer het opnieuw.";
            failed.ShowDialog();
        }

        public void ShowSaveSucceed()
        {
            SuccesDialogView succes = new SuccesDialogView();
            succes.getLabelSucces().Text = "Antwoord is succesvol opgeslagen.";
            succes.ShowDialog();
        }

        //Saves the answer given by the user and then goes to the next question.
        public void AnswerMCSaveHandler(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            Client.Model.UserAnswer ua = new Client.Model.UserAnswer();
            ua.PredefinedAnswer_Id = btn.ImageIndex;
            ua.Question_Id = mainForm.getCurrentMCQuestion().Id;
            ua.Pincode_Id = Client.Properties.Settings.Default.Session_Id.ToString();

            Factory.UserAnswerFactory uaf = new Factory.UserAnswerFactory();
            uaf.Save(ua, new ControlHandler(mainForm.timeLabel), saveMCAnswerCallBackHandler);
            if (mainForm.getQuestionList().MCQuestions.Count - 1 > 0)
            {
                if (!mainForm.getTempo())
                {
                    mainForm.goToNextQuestion();
                }
                else
                {
                    view.cleanUpPreviousQuestion();
                    view.initWaitScreen();
                }
            }
            else
            {
                view.cleanUpPreviousQuestion();
                view.initWaitScreen();
            }
        }


        //Saves the answer given by the user and then goes to the next question.
        public void AnswerOpenQuestionSaveHandler(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            Model.UserAnswerToOpenQuestion ua = new Model.UserAnswerToOpenQuestion();
            ua.Id = btn.ImageIndex;
            ua.Question_Id = mainForm.getCurrentOpenQuestion().Id;
            ua.Answer = mainForm.openQuestionBox.Text;
            ua.Student = "S123456";

            Factory.UserAnswerToOpenQuestionFactory uaf = new Factory.UserAnswerToOpenQuestionFactory();
            uaf.Save(ua, new ControlHandler(mainForm.timeLabel), saveOpenAnswerCallBackHandler);
            mainForm.setBusy(false);

            if (mainForm.getController().getCurrentQuestionIndex() < mainForm.getQuestionList().MCQuestions.Count + mainForm.getQuestionList().OpenQuestions.Count - 1)
            {
                mainForm.goToNextQuestion();
            }
            else
            {
                mainForm.getView().initWaitScreen();
            }
        }

        //This function adds the question (which is retrieved from the server) to the locally stored questionList
        public void Factory_questionAdded(Model.Question question)
        {
            this.mainForm.getQuestionList().MCQuestions.Add(question);
        }

        //This function adds the PredefinedAnswers to the question once a question is added
        private void PAFactory_predefinedAnswerAdded(Model.PredefinedAnswer answer)
        {
            Model.Question question = this.mainForm.getQuestionList().MCQuestions.Find(x => x.Id == answer.Question_Id);

            if (question.PredefinedAnswers == null)
            {
                question.PredefinedAnswers = new List<Model.PredefinedAnswer>();
            }

            question.PredefinedAnswers.Add(answer);
            
            if (!mainForm.isBusy() && question.PredefinedAnswers.Count == question.PredefinedAnswerCount)
            {
                mainForm.Invoke((Action)delegate () { mainForm.goToNextQuestion(); });
            }
        }

        //Subscribes the client to a group/session
        private void Client_connectionStatusChanged(StateChange message)
        {
            if (message.NewState == ConnectionState.Connected)
            {
                client.SubscribeList(mainForm.List_Id);
            }
        }

        public int getCurrentQuestionIndex()
        {
            return this.currentQuestionIndex;
        }

        public void setCurrentQuestionIndex(int index)
        {
            this.currentQuestionIndex = index;   
        }

    }
}
