using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using UnityEngine;

namespace Managers
{
    class TestManager : Singleton<TestManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcDefine.NpcFunction.InvokeShop, OnNpcInvokeShop);
            NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcDefine.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);          
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeShop：Npc:[{0} : {1}] Type : {2} Func : {3} ", npc.ID, npc.Name, npc.Type, npc.Function);
            UITest uITest = UIManager.Instance.Show<UITest>();
            uITest.SetTitle(npc.Name, "test");
            return true;
        }
        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeInsrance：Npc:[{0} : {1}] Type : {2} Func : {3} ", npc.ID, npc.Name, npc.Type, npc.Function);
            MessageBox.Show("点击了Npc：" + npc.Name, "Npc对话");
            return true;
        }

    }
}
