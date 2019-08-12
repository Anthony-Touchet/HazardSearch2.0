using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyID : MonoBehaviour
{
    public UnityEngine.UI.Button submitButton;
    public UnityEngine.UI.InputField idInput;
    public UnityEngine.UI.Text returnMesage;

    public UnityEngine.Events.UnityEvent onAccept;

    private void Start()
    {
        submitButton.onClick.AddListener(delegate(){StartCoroutine(CheckCompanyKey());});
    }

    IEnumerator CheckCompanyKey()
    {
        MironDB.MironDB_Manager.statusReturn = null;
        MironDB.MironDB_Manager.CheckKey(idInput.text);
        yield return new WaitWhile(() => MironDB.MironDB_Manager.statusReturn == null);

        if(MironDB.MironDB_Manager.statusReturn.status == "ok")
        {
            SaveCompanyID();
            onAccept.Invoke();
        }

        else
        {
            returnMesage.text = MironDB.MironDB_Manager.statusReturn.error_description.Split(new char[]{'.'})[0];
        }
    }

    public void SaveCompanyID()
    {
        MironDB.MironDB_Manager.machineID = idInput.text;
    }
}