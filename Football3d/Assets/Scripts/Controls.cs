using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    private string redTeamPlayerOnePass = "4";
    public string RedTeamPlayerOnePass {
        get {
            return redTeamPlayerOnePass;
        }
        set {
            redTeamPlayerOnePass = value;
        }
    }

    private string redTeamPlayerTwoPass = "5";
    public string RedTeamPlayerTwoPass {
        get {
            return redTeamPlayerTwoPass;
        }
        set {
            redTeamPlayerTwoPass = value;
        }
    }

}
