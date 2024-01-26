using System;
using System.Linq;
using UnityEngine;

namespace Dialogue_Scripts
{
    public class Action
    {
        private string _actionName;
        private string[] _parameterList;
        private ActionConstants _actionConstants;
        private string[] _allowableActions;
        
        public Action(string actionName)
        {
            _actionConstants = Resources.Load<ActionConstants>("ScriptableObjects/ActionConstants");
            _allowableActions = _actionConstants.actionList;
            if (! _allowableActions.Contains(actionName))
            {
                throw new Exception("@Action: Invalid action name " + actionName + "!");
            }
            _actionName = actionName;
        }

        public Action(string actionName, string[] parameterList)
        {
            _actionConstants = Resources.Load<ActionConstants>("ScriptableObjects/ActionConstants");
            _allowableActions = _actionConstants.actionList;
            if (! _allowableActions.Contains(actionName))
            {
                Debug.Log(actionName);
                Debug.Log("@Action: Invalid action name " + actionName + "!");
            }
            else
            {
                _actionName = actionName;
                _parameterList = parameterList;
            }
        }

        public string GetActionName()
        {
            return _actionName;
        }

        public string[] GetParameterList()
        {
            return _parameterList;
        }
    }
}