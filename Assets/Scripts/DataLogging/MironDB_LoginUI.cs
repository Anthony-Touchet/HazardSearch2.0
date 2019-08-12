using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MironDB
{
    public class MironDB_LoginUI : MonoBehaviour
    {
		public UnityEngine.UI.InputField m_inputEmail;
		public UnityEngine.UI.InputField m_inputPassword;
        
        public UnityEngine.UI.Button m_buttonLogin;
        public UnityEngine.UI.Button m_buttonForgotPassword;
        public UnityEngine.UI.Button m_buttonRegister;

        [Space]
        public UnityEngine.UI.Text m_textReturnMessage;

        [Space]
        public UnityEngine.Events.UnityEvent OnSuccess;


        void Start()
        {
           InitializeButtons();
        }

        void FixedUpdate()
        {
            if(MironDB_Manager.statusReturn == null) return;


            m_textReturnMessage.text = MironDB_Manager.statusReturn.error_description;
        }

        public void InitializeButtons()
        {
            if(m_buttonLogin != null)
            {
                m_buttonLogin.onClick.AddListener(delegate()
                {

                    StartCoroutine(LoginCheck());

                    // MironDB_Manager.Login(
                    // m_inputEmail.text, m_inputPassword.text, m_textReturnMessage);
                });
            }

            if(m_buttonForgotPassword != null)
            {
                m_buttonForgotPassword.onClick.AddListener(delegate()
                {
                    MironDB_Manager.ForgotPassword(m_inputEmail.text);
                });
            }
            
        }

        IEnumerator LoginCheck()
        {
            m_buttonLogin.interactable = false;

            MironDB_Manager.statusReturn = null;

            MironDB_Manager.Login(
                m_inputEmail.text, m_inputPassword.text);
        
            yield return new WaitWhile(() => MironDB_Manager.statusReturn == null);

            m_buttonLogin.interactable = true;

            if(MironDB_Manager.statusReturn.status.Contains("ok"))
            {
                OnSuccess.Invoke();
            }

            else
            {
                MironDB_Manager.Logout();
            }
        }
    }
}