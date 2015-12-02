﻿using Client.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.Factory
{
    public class UserAnswerFactory : AbstractFactory
    {
        #region Delegates
        public delegate void UserAnswerAdded(UserAnswer answer);
        public delegate void UserAnswerRemoved(UserAnswer answer);
        public delegate void UserAnswerUpdated(UserAnswer answer);
        #endregion

        #region Events
        public event UserAnswerAdded userAnswerAdded;
        public event UserAnswerRemoved userAnswerRemoved;
        public event UserAnswerUpdated userAnswerUpdated;
        #endregion

        #region Constants
        private const string resource = "PredefinedAnswers";
        #endregion

        #region Constructors
        public UserAnswerFactory()
        {
            this.signalRClient.proxy.On<UserAnswer>("UserAnswerAdded", this.onUserAnswerAdded);
            this.signalRClient.proxy.On<UserAnswer>("UserAnswerRemoved", this.onUserAnswerRemoved);
            this.signalRClient.proxy.On<UserAnswer>("UserAnswerUpdated", this.onUserAnswerUpdated);
        }
        #endregion

        #region Actions
        private void onUserAnswerAdded(UserAnswer a)
        {
            if (this.userAnswerAdded != null)
            {
                this.userAnswerAdded(a);
            }
        }

        private void onUserAnswerRemoved(UserAnswer a)
        {
            if (this.userAnswerRemoved != null)
            {
                this.userAnswerRemoved(a);
            }
        }

        private void onUserAnswerUpdated(UserAnswer a)
        {
            if (this.userAnswerUpdated != null)
            {
                this.userAnswerUpdated(a);
            }
        }
        #endregion

        #region Methods
        public void delete(UserAnswer answer, Control control, Action<UserAnswer> callback)
        {
            this.delete<UserAnswer>(answer.Id, resource, control, callback);
        }

        public void deleteAsync(UserAnswer answer, Action<UserAnswer> callback)
        {
            this.deleteAsync<UserAnswer>(answer.Id, resource, callback);
        }

        private Dictionary<string, object> getFields(UserAnswer answer)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("Question_Id", answer.Question_Id);
            values.Add("PredefinedAnswer_Id", answer.PredefinedAnswer_Id);

            return values;
        }

        public void saveAsync(UserAnswer answer, Action<UserAnswer> callback)
        {
            this.saveAsync<UserAnswer>(this.getFields(answer), resource, callback);
        }

        public void save(UserAnswer answer, Control control, Action<UserAnswer> callback)
        {
            this.save<UserAnswer>(this.getFields(answer), resource, control, callback);
        }

        public void findByIdAsync(int id, Action<UserAnswer> callback)
        {
            this.findByIdAsync<UserAnswer>(id, resource, callback);
        }

        public void findById(int id, Control control, Action<UserAnswer> callback)
        {
            this.findById<UserAnswer>(id, resource, control, callback);
        }

        public void findAll(Control control, Action<List<UserAnswer>> callback)
        {
            this.findAll<UserAnswer>(resource, control, callback);
        }

        public void findAllAsync(Action<List<UserAnswer>> callback)
        {
            this.findAllAsync<UserAnswer>(resource, callback);
        }
        #endregion
    }
}