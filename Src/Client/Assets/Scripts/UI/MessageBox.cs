using UnityEngine;

public class MessageBox
{
    static Object cacheObject = null;

    public static MessageBox_Handler Show(string message, string title = "", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (cacheObject == null)
        {
            cacheObject = Resloader.Load<Object>("PreFabs/Mian_City/UIMessageBox");
        }
    
        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        MessageBox_Handler msgbox = go.GetComponent<MessageBox_Handler>();
        msgbox.Init(title, message, type, btnOK, btnCancel);
        return msgbox;
    }
}

public enum MessageBoxType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    Error = 3
}