using System.Collections.Generic;
using UnityEngine;

namespace com.absence.consolesystem
{
    [System.Serializable]
    public class Command
    {
        [SerializeField] private string m_keyword = "::null";
        [SerializeField] private List<Argument> m_arguments = new();

        [SerializeField] private string m_description;
        [SerializeField] private string m_methodPreview = "";

#if UNITY_EDITOR
        [SerializeField] private bool m_isExpanded;
#endif

        public string Keyword => m_keyword.Trim();
        public List<Argument> Arguments => m_arguments;
        public string Description => m_description;
        public string MethodPreview => m_methodPreview;
    }

}