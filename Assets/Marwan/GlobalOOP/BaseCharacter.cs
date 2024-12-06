using UnityEngine;
namespace Retro.ThirdPersonCharacter
{   
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
    public class BaseCharacter : MonoBehaviour , IDamageable
    {
        // A virtual method for taking damage; does nothing by default.
        // Subclasses can override this to implement custom damage logic.
        public virtual void TakeDamage(int amount)
        {
            // Intentionally left blank
        }

        // A virtual method for healing; does nothing by default.
        // Subclasses can override this to implement custom healing logic.
        public virtual void Heal(int amount)
        {
            // Intentionally left blank
        }

        // A protected virtual method for handling death logic.
        // Subclasses can override this to implement custom death behaviors.
        protected virtual void Die()
        {
            // Intentionally left blank, can be overridden by subclasses.
        }
    }
}
