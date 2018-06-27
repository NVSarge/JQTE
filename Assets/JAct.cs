using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JAct", menuName = "Add JAction", order = 2)]
public class JAct : ScriptableObject
{
    public enum Types
    {
        QAtt,
        QDef,
        Quse
    };
    public Types T;
    public JBaseQte.Types JQT;
}
