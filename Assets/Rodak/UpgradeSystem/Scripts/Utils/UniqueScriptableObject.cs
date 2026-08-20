using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rodak.UpgradeSystem.Utils
{
    public abstract class UniqueScriptableObject<T> : ScriptableObject, IEquatable<UniqueScriptableObject<T>> where T : UniqueScriptableObject<T>
    {
        [SerializeField, HideInInspector] private string uniqueId;

        public string UniqueId => uniqueId;

        public bool Equals(UniqueScriptableObject<T> other)
        {
            if ((UnityEngine.Object)other == null) return false;
            return uniqueId == other.uniqueId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UniqueScriptableObject<T>)obj);
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(uniqueId) ? base.GetHashCode() : uniqueId.GetHashCode();
        }

        public static bool operator ==(UniqueScriptableObject<T> left, UniqueScriptableObject<T> right)
        {
            bool leftIsNull = (UnityEngine.Object)left == null;
            bool rightIsNull = (UnityEngine.Object)right == null;

            if (leftIsNull && rightIsNull) return true;
            if (leftIsNull || rightIsNull) return false;

            return left.Equals(right);
        }

        public static bool operator !=(UniqueScriptableObject<T> left, UniqueScriptableObject<T> right)
        {
            return !(left == right);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return;

            string guid = AssetDatabase.AssetPathToGUID(path);

            if (uniqueId != guid)
            {
                uniqueId = guid;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}