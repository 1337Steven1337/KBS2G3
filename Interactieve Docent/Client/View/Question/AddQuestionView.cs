﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.Controller;
using Client.Model;
using Client.Service.Thread;
using Client.View.Main;
using Client.Controller.Question;
using System.Net;
using Client.View.Dialogs;
using System.ComponentModel;
using System.Linq;
using MetroFramework.Forms;
using System.Drawing;
using Client.Service.SignalR;

namespace Client.View.Question
{
    public partial class AddQuestionView : MetroForm, IAddView<Model.Question>
    {
        private AddQuestionController Controller;
        private BindingList<Model.PredefinedAnswer> CurrentAnswersList = new BindingList<Model.PredefinedAnswer>();
        private BindingList<Model.PredefinedAnswer> OldAnswersList = new BindingList<Model.PredefinedAnswer>();
        private BindingList<Model.PredefinedAnswer> RightAnswerList = new BindingList<Model.PredefinedAnswer>();
        private Boolean Edit;
        private bool isTeacher = false;
        private Model.Question Question;
        
        public AddQuestionView(Model.Question question,bool isTeacherForm)
        {
            InitializeComponent();
            this.Question = question;
            this.isTeacher = isTeacherForm;

            btnSaveQuestion.Click += BtnSaveQuestion_Click;
            btnAddAnswer.Click += BtnAddAnswer_Click;
            btnDeleteAnswer.Click += BtnDeleteAnswer_Click;
            btnQuit.Click += BtnQuit_Click;

            answersListBox.DisplayMember = "Text";
            rightAnswerComboBox.DisplayMember = "Text";

            answerField.PreviewKeyDown += AnswerField_PreviewKeyDown;
            answersListBox.DataSource = CurrentAnswersList;
            rightAnswerComboBox.DataSource = RightAnswerList;

            //If question is not null, the user wants to edit a question.
            if (question != null)
            {
                EditQuestion(question);
                titleTile.Text = "Vraag wijzigen ";
                Edit = true;
            }
            else
            {
                titleTile.Text = "Nieuwe vraag";
                Edit = false;
            }
        }

        public TableLayoutPanel getTable()
        {
            return mainTablePanel;
        }

        //Loads the data from a selected question in Input fields
        private void EditQuestion(Model.Question question)
        {
            questionField.Text = question.Text;
            
            if(question.Time > 0)
            {
                timeField.Value = question.Time;
            }
            else
            {
                rbNoTime.Checked = true;
            }

            foreach (Model.PredefinedAnswer pa in question.PredefinedAnswers)
            {
                OldAnswersList.Add(pa);
                CurrentAnswersList.Add(pa);
                RightAnswerList.Add(pa);

                if(pa.Right_Answer)
                {
                    rightAnswerComboBox.SelectedItem = pa;
                }
            }
        }

        //Clears all input fields in Addquestionview
        public void ClearAllFields()
        {
            questionField.Text = "";
            timeField.Value = 10;
            CurrentAnswersList.Clear();
            answersListBox.DataSource = CurrentAnswersList;
            RightAnswerList.Clear();
            rightAnswerComboBox.DataSource = RightAnswerList;
        }

        private void AnswerField_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                BtnAddAnswer_Click(sender, e);
            }
        }

        //Close the third panel, Which contains the addquestion fields
        private void BtnQuit_Click(object sender, EventArgs e)
        {
            if (isTeacher == false)
            {
                Controller.InvokeRemoveQuestionPanel();
                this.Close();
            }
            else
            {
                SignalRClient.GetInstance().StopQuestionList(Properties.Settings.Default.Session_Id); //Stops the questionlist.
                Controller.InvokeRemoveQuestionPanel();
                this.Close();
            }

        }

        //Delete selected answer from AnswersList
        private void BtnDeleteAnswer_Click(object sender, EventArgs e)
        {
            CurrentAnswersList.Remove(GetSelectedAnswer());
            RightAnswerList.Remove(GetSelectedAnswer());
        }

        //Add answer to AnswersList
        private void BtnAddAnswer_Click(object sender, EventArgs e)
        {
            //Remove all whitespaces at beginning and end of the string
            String answer = answerField.Text;
            answer = answer.Trim();

            if (answer != "" && CurrentAnswersList.ToList().Find(x => x.Text == answer) == null)
            {
                Model.PredefinedAnswer pa = new Model.PredefinedAnswer() { Text = answer };
                CurrentAnswersList.Add(pa);
                RightAnswerList.Add(pa);
                this.answerField.Text = "";
                this.answerField.Focus();
            }                 
            else
            {
                FailedDialogView failed = new FailedDialogView();
                failed.getLabelFailed().Text = "Antwoordveld niet ingevuld of het antwoord bestaat al.";
                failed.ShowDialog();
            }
        }

        //Draw custom colors in Listbox
        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < answersListBox.Items.Count)
            {
                Graphics g = e.Graphics;

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.FromArgb(243, 119, 53) : Color.FromArgb(153, 153, 153));
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                PredefinedAnswer itemText = (PredefinedAnswer)answersListBox.Items[itemIndex];

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText.Text, e.Font, itemTextColorBrush, answersListBox.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void BtnSaveQuestion_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                DialogResult dr = new DialogResult();
                ConfirmDialogView confirm = new ConfirmDialogView();
                confirm.getLabelConfirm().Text = "Weet u zeker dat u de vraag wilt opslaan?";
                dr = confirm.ShowDialog();

                if (dr == DialogResult.Yes)
                {                    
                    Dictionary<string, object> iDictionary = new Dictionary<string, object>();
                    iDictionary.Add("Text", questionField.Text.Trim());
                    iDictionary.Add("Time", timeField.Value);
                    iDictionary.Add("PredefinedAnswerCount", this.answersListBox.Items.Count);
                    
                    if (Edit)
                    {
                        Dictionary<string, object> iDictionary2 = iDictionary;
                        iDictionary2.Add("Id", Question.Id);
                        this.Controller.UpdateQuestion(iDictionary2);
                    }
                    else
                    {
                        this.Controller.SaveQuestion(iDictionary);
                    }
                }             
            }
            else
            {
                FailedDialogView failed = new FailedDialogView();
                failed.getLabelFailed().Text = "U heeft nog niet alle velden ingevuld.";
                failed.ShowDialog();
            }
          
        }

        //Validate all inputfields
        private Boolean ValidateFields()
        {
            if (TimeIsSet())
            {
                //if time is smaller then 3, set time to standard 10 sec.
                if (timeField.Value < 3)
                {
                    timeField.Value = 10;
                }

                int Time = -1;

                try
                {
                    Time = Convert.ToInt32(timeField.Value);
                }
                catch (Exception)
                {

                }

                return (Time >= 3 && questionField.Text != "" && answersListBox.Items.Count > 0);
            }
            else
            {
                //time is not set
                timeField.Value = 0;
                return (questionField.Text != "" && answersListBox.Items.Count > 0);
            }
        }

        private Boolean TimeIsSet()
        {
            return (rbSetTime.Checked);
        }

        //Add view to mainTable
        public void AddToParent(IView parent)
        {
            MainView main = (MainView)parent;

            main.AddTablePanel(this.mainTablePanel, 3);
        }

        public IControlHandler GetHandler()
        {
            return new ControlHandler(this.answersListBox);
        }

        public void SetController(IController controller)
        {
            this.Controller = (AddQuestionController)controller;
        }

        //Get the Selelecteditem from Listbox
        public Model.PredefinedAnswer GetSelectedAnswer()
        {
            return (Model.PredefinedAnswer)this.rightAnswerComboBox.SelectedItem;
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
            succes.getLabelSucces().Text = "De vraag is succesvol opgeslagen.";
            succes.ShowDialog();
        }

        public void ShowSaveResult(Model.Question instance, HttpStatusCode status)
        {
            if(status == HttpStatusCode.Created && instance != null)
            {
                this.Controller.SavePredefinedAnswers(CurrentAnswersList.ToList(), instance, false);
            }
            else
            {
                this.ShowSaveFailed();
            }
        }

        public void ShowUpdateResult(Model.Question instance, HttpStatusCode status)
        {
            if (status == HttpStatusCode.NoContent && instance != null)
            {
                this.Controller.DeletePredefinedAnswers(OldAnswersList.ToList(), instance);
            }
            else
            {
                this.ShowSaveFailed();
            }
        }

        public void ShowDeleteAnswersResult(Model.Question instance, HttpStatusCode status)
        {
            if (status == HttpStatusCode.OK && instance != null)
            {
                this.Controller.SavePredefinedAnswers(CurrentAnswersList.ToList(), instance, true);
            }
            else
            {
                this.ShowSaveFailed();
            }
        }

        private void rbNoTime_CheckedChanged(object sender, EventArgs e)
        {
            if(rbNoTime.Checked == true)
            {
                labelTimeField.Visible = false;
                timeField.Visible = false;
            }
            else
            {
                labelTimeField.Visible = true;
                timeField.Visible = true;
            }
        }

    }
}
