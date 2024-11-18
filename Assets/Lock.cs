using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    
    private bool isLocked = false;
    public bool IsLocked => isLocked;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    
    private SpriteRenderer spriteRenderer;
    
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Position(Vector3 position)
    {
        transform.localPosition = position;
    }

    public Action<bool> OnLockUnlock;
    
    public void LockUnlock()
    {
        isLocked = !isLocked;
        spriteRenderer.sprite = isLocked ? lockedSprite : unlockedSprite;
        OnLockUnlock?.Invoke(isLocked);
    }
    
    public void LockUnlock(bool isLocked)
    {
        this.isLocked = isLocked;
        spriteRenderer.sprite = isLocked ? lockedSprite : unlockedSprite;
        OnLockUnlock?.Invoke(isLocked);
    }

    private void OnMouseDown()
    {
        LockUnlock();
    }
}
