using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Network;
using SkillBridge.Message;
using UnityEngine;

namespace Services
{
    class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);

        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        HashSet<StatusNotifyHandler> handlers = new HashSet<StatusNotifyHandler>();

        public void Init()
        { 
        
        }

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void RegisterStatusNofity(StatusType function, StatusNotifyHandler action)
        {
            if (handlers.Contains(action))
            {
                return;
            }
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
            handlers.Add(action);
        }        

        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach (NStatus status in notify.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            Debug.LogFormat("");
            if (status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                {
                    User.Instance.AddGold(status.Value);
                }
                else if(status.Action == StatusAction.Delete)
                {
                    User.Instance.AddGold(-status.Value);
                }
            }

            StatusNotifyHandler handler;

            if (eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}
