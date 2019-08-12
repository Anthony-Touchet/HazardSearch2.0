using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
    public class MironDB_HUB : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI m_username;

        public UnityEngine.UI.Button m_logout;

        IEnumerator Start()
        {
            m_logout.onClick.AddListener(delegate()
            {
                MironDB_Manager.Logout();
            });

            MironDB_Manager.currentUser = null;
            MironDB_Manager.GetuserInformation();

            yield return new WaitWhile(() => MironDB_Manager.currentUser == null);
            m_username.text = MironDB_Manager.currentUser.name;
        }
    }
}