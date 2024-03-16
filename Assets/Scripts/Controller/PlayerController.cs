using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;


namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {

        Health healt;
        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;

        void Start()
        {
            healt = GetComponent<Health>();
        }

        private void Update()
        {
            if (healt.IsDead() == true)
            {
                return;
            }
            if (InteractWithEnemy())
            {
                return;
            }
            if (InteractWithMovement())
            {
                return;
            }
            SetCursor(CursorType.None);
        }
        bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Move>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor (CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


            private bool InteractWithEnemy () 
            {
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                foreach (var hit in hits)
                {
                    
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target == null)
                    {
                        continue;
                    }

                    if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    {
                        continue;
                    }
                    if (target == null)
                    {
                        continue;
                    }
                    if (Input.GetMouseButton(1))
                    {
                        GetComponent<Fighter>().Attack(target.gameObject);
                    }
                    SetCursor(CursorType.Combat);
                        return true;
                }
                return false;
            }
        


    }
    }



