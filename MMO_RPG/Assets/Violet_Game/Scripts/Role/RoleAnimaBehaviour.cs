using UnityEngine;

public class RoleAnimaBehaviour : MonoBehaviour
{
    private RoleCtrlBase _roleCtrl;
    [SerializeField] private Transform _effectPos;

    private void Awake()
    {
        _roleCtrl = GetComponentInParent<RoleCtrlBase>();
    }

    private void AnimaEnd()
    {
        _roleCtrl.ChangeState(RoleState.Idle);
    }

    private void PlayEffect(string tag)
    {
        switch (tag)
        {
            case "atk01":
                ResourceMgr.Instance.LoadEffectAsync("Attack/Attack01/Effect_Attack01", (GameObject go) =>
                {
                    go.transform.SetParent(_effectPos);
                    go.transform.localPosition = new Vector3(-0.18f, -0.58f, -0.58f);
                    go.transform.localEulerAngles = new Vector3(40.97f, 83.41f, 216.81f);
                    go.transform.localScale = Vector3.one;

                });
                break;
            case "atk02":
                ResourceMgr.Instance.LoadEffectAsync("Attack/Attack01/Effect_Attack01", (GameObject go) =>
                {
                    go.transform.SetParent(_effectPos);
                    go.transform.localPosition = new Vector3(0.09f, -0.2f, -0.17f);
                    go.transform.localEulerAngles = new Vector3(270f, 35.54f, 0);
                    go.transform.localScale = Vector3.one;

                });
                break;
            case "atk03_1":
                ResourceMgr.Instance.LoadEffectAsync("Attack/Attack01/Effect_Attack01", (GameObject go) =>
                {
                    go.transform.SetParent(_effectPos);
                    go.transform.localPosition = new Vector3(-0.33f, 0.5f, -0.45f);
                    go.transform.localEulerAngles = new Vector3(51.62f, 262.38f, 99.87f);
                    go.transform.localScale = Vector3.one;

                });
                break;
            case "atk03_2":
                ResourceMgr.Instance.LoadEffectAsync("Attack/Attack01/Effect_Attack01", (GameObject go) =>
                {
                    go.transform.SetParent(_effectPos);
                    go.transform.localPosition = new Vector3(0.92f, 0.36f, 0.07f);
                    go.transform.localEulerAngles = new Vector3(298.69f, 254.45f, 98.72f);
                    go.transform.localScale = Vector3.one;

                });
                break;
            default:
                break;
        }
    }
}
