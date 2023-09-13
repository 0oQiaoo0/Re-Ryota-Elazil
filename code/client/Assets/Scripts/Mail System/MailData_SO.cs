using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mail", menuName = "Mail/Mail Data")]
public class MailData_SO : ScriptableObject
{
    public List<MailPiece> mailPieces = new List<MailPiece>();
}
