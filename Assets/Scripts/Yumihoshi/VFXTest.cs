// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/04/09 13:04
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections;
using HoshiVerseFramework.Components.Factory;
using UnityEngine;
using Yumihoshi.Singletons;
using Yumihoshi.UI.GetObjTip;

namespace Yumihoshi
{
    public class VFXTest : MonoBehaviour
    {
        [SerializeField] private TipHandler _tipHandler;

        private void Start()
        {
            StartCoroutine(StartTest());
        }

        private IEnumerator StartTest()
        {
            yield return new WaitForSeconds(1f);
            VFXFactory.Instance.Factory.CreateVFX(VFXType.ObjVanishStar2Small)
                .Play();
            yield return new WaitForSeconds(1f);
            _tipHandler.ShowTip("星辰+1");
            yield return new WaitForSeconds(1f);
            _tipHandler.ShowTip("星辰+1");
            yield return new WaitForSeconds(1f);
            for (var i = 0; i < 10; i++)
            {
                _tipHandler.ShowTip("星辰+" + i);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
