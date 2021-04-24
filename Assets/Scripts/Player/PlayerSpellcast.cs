using UnityEngine;


public class PlayerSpellcast : MonoBehaviour
{
    [SerializeField] private Transform _spellSpawn;
    [SerializeField] private GameObject _spellObj;

    
    
    public void CastSpell(ESoundPitch[] list)
    {
        GameObject spell = Instantiate(_spellObj,_spellSpawn.position,Quaternion.identity);
        spell.GetComponent<Spell>().AddForce(transform.forward);
    }
}
