using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbilityMock : ActiveAbility
{
    private ActiveAbilityMockSO ActiveAbilityMockSO => AbilitySO as ActiveAbilityMockSO;

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion
}
