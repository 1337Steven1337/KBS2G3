﻿using Client.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client.Factory
{
    public interface IFactory<T> where T : AbstractModel, new()
    {
        void SetResource(string resource);
        void DeleteAsync(T instance, Action<T, HttpStatusCode, IRestResponse> callback);
        void SaveAsync(List<KeyValuePair<string, object>> data, Action<T, HttpStatusCode, IRestResponse> callback);
        void FindByIdAsync(int id, Action<T, HttpStatusCode, IRestResponse> callback);
        void FindAllAsync(Action<List<T>, HttpStatusCode, IRestResponse> callback);

    }
}