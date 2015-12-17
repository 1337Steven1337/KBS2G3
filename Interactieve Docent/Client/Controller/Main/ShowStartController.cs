﻿using System;
using Client.Factory;
using Client.View;
using System.Net;
using Client.Service.SignalR;
using Client.View.Main;

namespace Client.Controller.Main
{
    public class ShowStartController : AbstractController<Model.Pincode>
    {
        private IStartView View { get; set; }
        private PincodeFactory PincodeFactory = new PincodeFactory();

        public ShowStartController(IView view)
        {
            this.SetView(view);
            view.SetController(this);
        }

        private void UseCode(Model.Pincode pincode, HttpStatusCode status)
        {
            this.View.ShowCodeResult(pincode, status);

            if (pincode != null && status == HttpStatusCode.OK)
            {
                SignalRClient.GetInstance().SubscribePincode(pincode);


            }
        }

        public void ApplyTestCode(string code)
        {
            this.CheckCode(code);
        }

        public override IView GetView()
        {
            return this.View;
        }

        public override void SetBaseFactory(IFactory<Model.Pincode> baseFactory)
        {
            PincodeFactory.SetBaseFactory(baseFactory);
        }

        public override void SetView(IView view)
        {
            this.View = (IStartView)view;
        }

        public void CheckCode(string code)
        {
            this.PincodeFactory.FindById(code, this.View.GetHandler(), this.UseCode);
        }
    }
}
