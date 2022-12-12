using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ProductHolder : MonoBehaviour
{
    public Product product;
    public bool blocked = false;

    private void OnDestroy() {
        Addressables.ReleaseInstance(gameObject);
    }
}
